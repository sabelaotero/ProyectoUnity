using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : Singleton<GamePlayManager>
{
    public float tiempo;

    private void Start()
    {
        tiempo = 180;
    }

    private void Update()
    {
        if(tiempo > 0)
        {
            tiempo -= Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(2);
        }

    }

}
