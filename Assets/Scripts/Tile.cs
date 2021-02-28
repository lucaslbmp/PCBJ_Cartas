using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false;
    public Sprite originalCarta; // sprite da carta desejada
    public Sprite backCarta; // sprite do verso da carta

    // Start is called before the first frame update
    void Start()
    {
        EscondeCarta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        print("Voce pressionou a telha");
        //if (tileRevelada)
        //{
        //    EscondeCarta();
        //}
        //else
        //{
        //    RevelaCarta();
        //}
        GameObject.Find("gameManager").GetComponent<GameManager>().CartaSelecionada(gameObject);
    }

    public void EscondeCarta()
    {
        GetComponent<SpriteRenderer>().sprite = backCarta;
        tileRevelada = false;
    }

    public void RevelaCarta()
    {
        GetComponent<SpriteRenderer>().sprite = originalCarta;
        tileRevelada = true;
    }

    public void setOriginalCarta(Sprite novaCarta)
    {
        originalCarta = novaCarta;
    }
}
