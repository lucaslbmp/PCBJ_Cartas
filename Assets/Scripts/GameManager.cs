using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject carta; // carta a ser descartada
    string[] naipes = {"clubs","hearts","spades","diamonds"}; //naipes possiveis no jogo (um naipe para cada linha)
    //string[] naipes = {"clubs","hearts"}; // naipes possiveis no jogo (um naipe para cada linha)
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // indicadores para a carta escolhida em cada linha a cada rodada
    private GameObject carta1, carta2; // gameObjects das cartas escolhidas
    string linhaCarta1, linhaCarta2; // armazenam os numeros das linhas de cada carta selecionada

    bool timerPausado,timerAcionado; // indicadores de pausa do Timer que conta o tempo até o jogador poder selecionar outra carta
    float timer;

    int numTentativas = 0; // Numero de tentativas na rodada
    int numAcertos = 0; // Numero de matches de pares
    AudioSource somOk; // Som de acerto

    int ultimoJogo = 0;
    //int recordeDeTentativas;
    float tempo;
    float melhorTempo;
    //public GameObject pontuacaoPrefab;

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas(); // Funçao que gera os baralhos de cartas na tela  
        UpdateNumTentativas(); // Atualizando o numero de tentativas
        somOk = GetComponent<AudioSource>(); // Obtendo o AudioSource correpondente ao som de acerto
        ultimoJogo = PlayerPrefs.GetInt("Jogadas",0); // Recuperando o numero de jogadas da partida anterior
        melhorTempo = StringParaSegundos(PlayerPrefs.GetString("MelhorTempo","00:00.00")); // Recuperando o melhor tempo de partida (recorde)
        Debug.Log(melhorTempo);
        //recordeDeTentativas = PlayerPrefs.GetInt("Record");
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo Anterior: " + ultimoJogo; // Atualizando na tela o numero de tentativas do ultimo jogo
        //GameObject.Find("recorde").GetComponent<Text>().text = "Recorde: " + recordeDeTentativas;
        GameObject.Find("melhorTempo").GetComponent<Text>().text = "Melhor Tempo: " + SegundosParaString(melhorTempo); // Atualizando o melhor tempo na tela
        tempo = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerAcionado)
        {
            timer += Time.deltaTime; //Incrementando o timer que determina o tempo até poder selecionar outra carta
            //print(timer);
            if (timer > 1) // Quando houver se passado 1 segundo:
            {
                timerPausado = true; // pausar o timer de seleção da carta
                timerAcionado = false;
                if(carta1.tag == carta2.tag) // Se as cartas forem iguais (acerto):
                {
                    Destroy(carta1); // Destruir 1a carta 
                    Destroy(carta2); // Destruir 2a carta
                    numAcertos++; // Incrementar numero de acertos
                    if (numAcertos == 26) // Se o numero de acertos chegar a 26 (o que significa vitória):
                    {
                        PlayerPrefs.SetInt("Jogadas",numTentativas); // Atualizar o numero de jogadas em PlayerPrefs
                        //if (recordeDeTentativas > 0)
                        //    recordeDeTentativas = Mathf.Min(numTentativas, recordeDeTentativas);
                        //else
                        //    recordeDeTentativas = numTentativas;
                        float tempoDaPartida = tempo; // Armazenar o tempo decorrido de partida
                        if (melhorTempo > 0) // Se o melhor tempo for maior que 0 (i.e., nao é a primeira partida)
                            melhorTempo = Mathf.Min(tempoDaPartida, melhorTempo); // O melhor tempo é atualizado para o menor valor entre o melhor tempo (recorde) e o tempo da partida atual
                        else // caso contrario
                            melhorTempo = tempoDaPartida; // o novo melhor tempo é o tempo da partida atual
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        //PlayerPrefs.SetInt("Record", recordeDeTentativas);
                        PlayerPrefs.SetString("MelhorTempo",SegundosParaString(melhorTempo)); // Atualizando a variavel MelhorTempo em PlayerPrefs 
                        PlayerPrefs.SetString("Tempo", SegundosParaString(tempoDaPartida)); // Atualizando a variavel Tempo em PlayerPrefs
                        if (tempoDaPartida > melhorTempo) // Se o tempo da partida atual é maior que o melhor tempo
                            SceneManager.LoadScene("TelaParabens"); // Carregar a cena que parabeniza o jogador pela vitoria
                        else // caso contrario
                            SceneManager.LoadScene("TelaRecorde"); // Carregar a cena que parabeniza o jogador pelo novo recorde
                    }
                    somOk.Play(); // Executar som de acerto
                }
                else // Se as cartas forem de valores diferentes (erro):
                {
                    carta1.GetComponent<Tile>().EscondeCarta(); // Virar carta 1
                    carta2.GetComponent<Tile>().EscondeCarta(); // Virar carta 2
                }
                primeiraCartaSelecionada = false; // Desfazer a seleçao da 1a carta
                segundaCartaSelecionada = false; // Desfazer a seleçao da 1a carta
                carta1 = null; // 
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0; // Zerando o timer que determina o tempo até poder selecionar outra carta
            }
        }
    }

    private void FixedUpdate()
    {
        tempo += Time.deltaTime; // Incrementando o tempo
        GameObject.Find("timer").GetComponent<Text>().text = SegundosParaString(tempo); // Atualizando o tempo na tela
    }

    void MostraCartas() // Funçao que gera os baralhos de cartas na tela 
    {
        for(int l = 0; l < naipes.Length; l++) // Para cada linha de cartas:
        {
            int[] arrayEmbaralhado = criaArrayEmbaralhado(); // Gerar um array de valores de cartas embaralhado 
            for (int i = 0; i < 13; i++)
            {
                AddUmaCarta(l, i, arrayEmbaralhado[i]); // Adicionar cada carta do array embaralhado
            }
        }
    }

    public void AddUmaCarta(int linha,int rank,int valor) // Adiciona um gameObject carta
    {
        GameObject centro = GameObject.Find("centroDaTela"); // Objeto que contem a posição do centro da tela
        float escalaCartaOriginal = carta.transform.localScale.x; // Dimensao da carta no eixo x
        float fatorEscalaX = (658 * escalaCartaOriginal) / 110f; // Parametro relacionado ao espaçamento em x da carta
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110f;// Parametro relacionado ao espaçamento em y da carta
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * fatorEscalaX, 
                                          centro.transform.position.y + (linha-(naipes.Length)/2)*fatorEscalaY, 
                                          centro.transform.position.z); // Posiçao da carta a ser adicionada
        //GameObject c = (GameObject)Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject c = (GameObject)Instantiate(carta, novaPosicao, Quaternion.identity); // Instanciando o objeto carta na posiçao escolhida
        c.tag = "" + (valor+1); // Tag da carta (ace,2-10,jack,queen,king), que é determinada pelo seu valor 
        c.name = "" + linha + "_" + valor; // Nome da carta: [linha]_[valor]
        string numeroDaCarta = "";
        string nomeDaCarta = "";
        // Escolhendo o numero da carta com base no seu valor:
        if (valor == 0)
            numeroDaCarta = "ace";
        else if (valor == 10)
            numeroDaCarta = "jack";
        else if (valor == 11)
            numeroDaCarta = "queen";
        else if (valor == 12)
            numeroDaCarta = "king";
        else
            numeroDaCarta = "" + (valor + 1);
        nomeDaCarta = numeroDaCarta + "_of_" + naipes[linha]; // Nome da carta cujo sprite será procurado: [numero da carta]_of_[naipe]
        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta); // Procurando o sprite corresponedente à carta procurada
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().setOriginalCarta(s1); // Atribuindo o sprite à carta adicionada
    }

    public int[] criaArrayEmbaralhado() // Funçao que cria um array com numeros de cartas organizados aleatoriamente
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; // Array de numeros de cartas
        int temp; // Variavel temporaria
        for(int t = 0; t < 13; t++) // Para cada posiçao de carta t:
        {
            temp = novoArray[t]; // Armazenando o numero da carta na posição t na variavel temporaria
            int r = UnityEngine.Random.Range(t,13); // Selecionando uma posiçao aleatoria de carta no descarte
            novoArray[t] = novoArray[r]; // O número da posição t recebe o número da carta na posição aleatoriamente selecionada
            novoArray[r] = temp; // O número da carta na posiçao aleatoriamente selecionada recebe o número da carta na posição t
        }
        return novoArray; // Retornando array de cartas embaralhado
    }

    public void CartaSelecionada(GameObject carta) // Funçao que recebe uma carta e gerencia a seleção da mesma:
    {
        if (!primeiraCartaSelecionada) // Ao clicar na primeira carta:
        {
            string linha = carta.name.Substring(0,1); 
            linhaCarta1 = linha; // A linha da carta 1 é armazenada
            primeiraCartaSelecionada = true; // Flag sinaliza que a primeira carta foi selecionada
            carta1 = carta; // A primeira carta é armazenada 
            carta1.GetComponent<Tile>().RevelaCarta(); // A carta é revelada
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta2 = linha; // A linha da carta 1 é armazenada
            segundaCartaSelecionada = true; // Flag sinaliza que a segunda carta foi selecionada
            carta2 = carta; // A segunda carta é armazenada 
            carta2.GetComponent<Tile>().RevelaCarta(); // A carta é revelada
            VerificaCartas(); // Atualiza o numero de tentativas
        }
    }

    public void VerificaCartas() // Funçao que atualiza o numero de tentativas
    {
        disparaTimer(); // Timer de seleção de carta é disparado
        numTentativas++; // Numero de tentativas é incrementado
        UpdateNumTentativas(); // Numero de tentativas é atualizado na tela
    }

    public void disparaTimer() // Funçao que controla as flags do timer de seleção de cartas
    {
        timerPausado = false;
        timerAcionado = true;
    }

    public void UpdateNumTentativas() // Função que atualiza o numero de tentativas na tela
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas;
    }

    public float StringParaSegundos(string str) // Funçao que converte uma string de cronometro no formato "mm:ss.ff" para segundos (float)
    {
        string[] split = str.Split(new char[]{ ':','.'}); // Dividindo a string em um array de strings com 3 termos [min,seg,fracseg]
        int[] relogio = Array.ConvertAll(split, int.Parse); // Convertendo array de strings em um array de inteiros
        return relogio[0] * 60f + relogio[1] + relogio[2] / 100f; // Convertendo para segundos (float) -> min*60 + seg + fracseg/100
    }

    public string SegundosParaString(float tempo) // Funçao que converte de segundos (float) para uma string de cronometro no formato "mm:ss.ff"
    {
        int min = (int)tempo / 60; // Calculando os minutos do cronometro
        int seg = (int)tempo % 60; // Calculando os segundos do cronometro
        int fracseg = (int)((tempo - (float)(seg + min * 60)) * 100); // Calculando centesimos de segundos do cronometro
        return min.ToString().PadLeft(2, '0') + ":" + 
            seg.ToString().PadLeft(2, '0') + "." + 
            fracseg.ToString().PadLeft(2, '0'); // Retornado uma string no formato "[min]:[seg].[centesimos]" com dois algarismos cada
    }
}
