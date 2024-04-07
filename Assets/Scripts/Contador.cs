using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Contador : MonoBehaviour
{
    public GamePlayManager timer;
    public float contador;
    public Text Textocontador;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<GamePlayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        contador = timer.tiempo;
        DisplayTime(contador);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float minutos = Mathf.FloorToInt(timeToDisplay / 60);
        float segundos = Mathf.FloorToInt(timeToDisplay % 60);
        if (Textocontador != null)
        {
            Textocontador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

}