using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject carta; // carta a ser descartada

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
        int[] arrayEmbaralhado = criaArrayEmbaralhado();
        int[] arrayEmbaralhado2 = criaArrayEmbaralhado();
        //Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 0; i < 13; i++)
        {
            //AddUmaCarta(i);
            AddUmaCarta(0,i,arrayEmbaralhado[i]);
            AddUmaCarta(1,i, arrayEmbaralhado2[i]);
        }
        
    }

    public void AddUmaCarta(int linha,int rank,int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (658 * escalaCartaOriginal) / 110f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110f;
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * 1.8f, centro.transform.position.y, centro.transform.position.z);
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + (rank - 13 / 2) * fatorEscalaX, centro.transform.position.y + (linha-2/2)*fatorEscalaY, centro.transform.position.z);
        //GameObject c = (GameObject)Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject c = (GameObject)Instantiate(carta, novaPosicao, Quaternion.identity);
        c.tag = "" + (valor+1);
        c.name = "" + linha + "_" + valor;
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
        nomeDaCarta = numeroDaCarta + "_of_clubs";
        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta);
        print("S: " + s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().setOriginalCarta(s1);
    }

    public int[] criaArrayEmbaralhado()
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for(int t = 0; t < 13; t++) // Para cada posi�ao de carta t:
        {
            temp = novoArray[t]; // Armazenar o numnero da carta na posi��o t numa variavel temporaria
            int r = Random.Range(t,13); // Selecionar uma posi�ao aleatoria de carta no descarte
            novoArray[t] = novoArray[r]; // O n�mero da posi��o t recebe o n�mero da carta na posi��o aleatoriamente selecionada
            novoArray[r] = temp; // O n�mero da carta na posi�ao aleatoriamente selecionada recebe o n�mero da carta na posi��o t
        }
        return novoArray;
    }
}