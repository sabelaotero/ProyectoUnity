using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ObjetosRecogidos objetosRecogidos = other.GetComponent<ObjetosRecogidos>();

        if (objetosRecogidos != null)
        {
            objetosRecogidos.ObjRecogido();
            gameObject.SetActive(false);
        }
    }
}
