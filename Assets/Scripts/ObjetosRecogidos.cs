using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ObjetosRecogidos : MonoBehaviour
{

    public GamePlayManager manager;
    public int numObjetos { get; private set; }
    

    public UnityEvent<ObjetosRecogidos> OnObjetoRecogido;

    public void ObjRecogido()
    {
        numObjetos++;
        OnObjetoRecogido.Invoke(this);
    }

    //public void Update()
    //{
      //  if (numObjetos == 10)
        //{
          //  SceneManager.LoadScene(1);
      //  }
    //}
     

}
