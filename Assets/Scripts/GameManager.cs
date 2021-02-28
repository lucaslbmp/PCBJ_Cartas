using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject carta; // carta a ser descartada
    //string[] naipes = {"clubs","hearts","spades","diamonds"};
    string[] naipes = {"clubs","hearts"}; // naipes possiveis no jogo (um naipe para cada linha)
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // indicadores para a carta escolhida em cada linha a cada rodada
    private GameObject carta1, carta2; // gameObjects das cartas escolhidas
    string linhaCarta1, linhaCarta2; // armazenam os numeros das linhas de cada carta selecionada

    bool timerPausado,timerAcionado; // indicadores de pausa do Timer que conta o tempo até o jogador poder selecionar outra carta
    float timer;

    int numTentativas = 0; // Numero de tentativas na rodada
    int numAcertos = 0; // Numero de matches de pares
    AudioSource somOk; // som de acerto

    int ultimoJogo = 0;

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas();
        UpdateNumTentativas();
        somOk = GetComponent<AudioSource>();
        ultimoJogo = PlayerPrefs.GetInt("Jogadas",0);
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo anterior: " + ultimoJogo;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if (timer > 1)
            {
                timerPausado = true;
                timerAcionado = false;
                if(carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    if (numAcertos == 13)
                    {
                        PlayerPrefs.SetInt("Jogadas",numTentativas);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    somOk.Play();
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostraCartas()
    {
        for(int l = 0; l < naipes.Length; l++) // Para cada linha de cartas:
        {
            int[] arrayEmbaralhado = criaArrayEmbaralhado(); // Gerar um array de cartas embaralhado 
            //int[] arrayEmbaralhado2 = criaArrayEmbaralhado();
            //Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
            for (int i = 0; i < 13; i++)
            {
                //AddUmaCarta(i);
                //AddUmaCarta(0, i, arrayEmbaralhado[i]);
                //AddUmaCarta(1, i, arrayEmbaralhado2[i]);
                AddUmaCarta(l, i, arrayEmbaralhado[i]); // Adicionar cada carta do array
            }
        }
    }

    public void AddUmaCarta(int linha,int rank,int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela"); // Objeto que contem a posição do centro da tela
        float escalaCartaOriginal = carta.transform.localScale.x; // Dimensao da carta no eixo x
        float fatorEscalaX = (658 * escalaCartaOriginal) / 110f; // Parametro relacionado às dimesoes da carta
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110f;
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * 1.8f, centro.transform.position.y, centro.transform.position.z);
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * fatorEscalaX, centro.transform.position.y + (linha-(naipes.Length)/2)*fatorEscalaY, centro.transform.position.z); // Posiçao da carta a ser adicionada
        //GameObject c = (GameObject)Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject c = (GameObject)Instantiate(carta, novaPosicao, Quaternion.identity); // Instanciando o objeto carta
        c.tag = "" + (valor+1); // Tag da carta (ace,2-10,jack,queen,king), que é determinada pelo seu valor 
        c.name = "" + linha + "_" + valor; // Nome da carta: [linha]_[valor]
        string numeroDaCarta = "";
        string nomeDaCarta = "";
        /*if (rank == 0)
            numeroDaCarta = "ace";
        else if (rank == 10)
            numeroDaCarta = "jack";
        else if (rank == 11)
            numeroDaCarta = "queen";
        else if (rank == 12)
            numeroDaCarta = "king";
        else
            numeroDaCarta = "" + (rank+1);
        nomeDaCarta = numeroDaCarta + "_of_clubs";
        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta);
        print("S: " + s1);
        GameObject.Find(""+rank).GetComponent<Tile>().setOriginalCarta(s1);*/
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
        nomeDaCarta = numeroDaCarta + "_of_" + naipes[linha]; // Nome da carta cujo sprite será procurado
        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta); // Procurando o sprite corresponedente à carta procurada
        print("S: " + s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().setOriginalCarta(s1); // Atribuindo o sprite escolhido ao objeto da carta adicionada
    }

    public int[] criaArrayEmbaralhado()
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for(int t = 0; t < 13; t++) // Para cada posiçao de carta t:
        {
            temp = novoArray[t]; // Armazenar o numero da carta na posição t numa variavel temporaria
            int r = Random.Range(t,13); // Selecionar uma posiçao aleatoria de carta no descarte
            novoArray[t] = novoArray[r]; // O número da posição t recebe o número da carta na posição aleatoriamente selecionada
            novoArray[r] = temp; // O número da carta na posiçao aleatoriamente selecionada recebe o número da carta na posição t
        }
        return novoArray; // Retornando array de cartas
    }

    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0,1);
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta2 = linha;
            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();
            VerificaCartas();
        }
    }

    public void VerificaCartas()
    {
        disparaTimer();
        numTentativas++;
        UpdateNumTentativas();
    }

    public void disparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }

    public void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas;
    }
}
