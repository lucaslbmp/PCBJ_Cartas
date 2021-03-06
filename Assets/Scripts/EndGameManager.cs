using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    void Start()
    {
        int tentativas = PlayerPrefs.GetInt("Jogadas",0); // Obtem numero de tentativas armazenado em PlayerPrefs
        string tempo = PlayerPrefs.GetString("Tempo",""); // Obtem tempo de partida armazenado em PlayerPrefs
        Debug.Log(tentativas + "," + tempo);
        GameObject.Find("tentativas").GetComponent<Text>().text = "" + tentativas; // Coloca numero de tentativas na tela
        GameObject.Find("tempo").GetComponent<Text>().text = "" + tempo; // Coloca tempo da partida na tela
    }


}
