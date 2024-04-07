using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fin: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ObjetosRecogidos objetosRecogidos = other.GetComponent<ObjetosRecogidos>();

        if (objetosRecogidos.numObjetos == 10)
        {
            SceneManager.LoadScene(2);
        }
    }
}