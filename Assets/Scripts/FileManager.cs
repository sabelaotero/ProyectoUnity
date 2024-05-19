using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class FileManager
{
    public static bool WriteToFile(string filname, string data)
    {
        string fullPath = Application.dataPath + "/" + filname;
        try
        {
            File.WriteAllText(fullPath, data);
            Debug.Log("Fichero guardado correctamente en: " + fullPath);
            return true;
        }
        catch(Exception e)
        {
            Debug.Log("Error al guardar el fichero en: " + fullPath + " con el error " + e);
            return false;
        }
    }
}
