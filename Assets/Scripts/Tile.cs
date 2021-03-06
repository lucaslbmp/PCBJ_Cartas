using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false; // flag que sinaliza se a carta foi revelada
    public Sprite originalCarta; // sprite da carta desejada
    public Sprite backCarta; // sprite do verso da carta

    void Start()
    {
        EscondeCarta(); // Todas as cartas iniciam escondidas
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown() // Ao clicar na carta
    {
        // Passar carta selecionada para a fun��o que gerencia a sele�ao de cartas:
        GameObject.Find("gameManager").GetComponent<GameManager>().CartaSelecionada(gameObject);
    }

    public void EscondeCarta() // Fun�ao que coloca a carta virada de costas
    {
        GetComponent<SpriteRenderer>().sprite = backCarta; // 
        tileRevelada = false;
    }

    public void RevelaCarta() // Fun�ao que coloca a carta virada para cima
    {
        GetComponent<SpriteRenderer>().sprite = originalCarta;
        tileRevelada = true;
    }

    public void setOriginalCarta(Sprite novaCarta) // Fun�ao que armazena um gameObject carta novo em originalCarta
    {
        originalCarta = novaCarta;
    }
}
