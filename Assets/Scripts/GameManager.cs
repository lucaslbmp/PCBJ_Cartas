using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject carta; // carta a ser descartada
    string[] naipes = {"clubs","hearts","spades","diamonds"};

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * fatorEscalaX, centro.transform.position.y + (linha-1.55f)*fatorEscalaY, centro.transform.position.z); // Posiçao da carta a ser adicionada
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
        return novoArray;
    }
}
