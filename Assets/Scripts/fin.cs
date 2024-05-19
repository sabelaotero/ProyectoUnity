using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fin: MonoBehaviour
{
    public ObjetosRecogidos objs;
    public DBManager dbman;
    private void OnTriggerEnter(Collider other)
    {
        ObjetosRecogidos objetosRecogidos = other.GetComponent<ObjetosRecogidos>();

        if (objetosRecogidos.numObjetos == 10)
        {
            int punt = objetosRecogidos.numObjetos;
            dbman.AñadirPuntos(punt);
            SceneManager.LoadScene(2);
        }
    }
}