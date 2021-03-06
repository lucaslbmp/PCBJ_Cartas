using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    public void RestartMundo() // Funçao chamada ao se clicar no botao "Restart"
    {
        SceneManager.LoadScene("Game"); // Carregar cena do jogo
    }

    public void EndMundo() // Funçao chamada ao se clicar no botao "Sair"
    {
        SceneManager.LoadScene("End"); // Carregar creditos
    }
}
