using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoObj : MonoBehaviour
{
    private TextMeshProUGUI objText;


    // Start is called before the first frame update
    void Start()
    {
        objText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void UpdateText(ObjetosRecogidos obj)
    {
        objText.text = obj.numObjetos.ToString();
    }
}
