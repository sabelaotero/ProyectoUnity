using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Choque : MonoBehaviour
{
    public ObjetosRecogidos objs;
    public DBManager dbman;
    private void OnTriggerEnter(Collider other)
    {

        ObjetosRecogidos objetosRecogidos = other.GetComponent<ObjetosRecogidos>();
        int punt = objetosRecogidos.numObjetos;
        dbman.AñadirPuntos(punt);
        SceneManager.LoadScene(2);
    }
}