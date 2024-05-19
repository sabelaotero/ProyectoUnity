using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

public class MenuFinal: MonoBehaviour
{
    public DBManager manager;
    public MenuInicial inicio;
    public List<Jugador> listaJugadores;

    public void reInicio()
    {
        int jugId = manager.IdJugadorUltPart();
        manager.AñadirPart(jugId);
        SceneManager.LoadScene(1);
    }

    public void salir()
    {
        listaJugadores = manager.TodosJug();
        generarJSON(listaJugadores);
        generarXML(listaJugadores);
        manager.CerrarDB();
        Application.Quit(0);
    }

    

    private void generarJSON(List<Jugador> listaJugadores)
    {
        listaJugadores = manager.TodosJug();
        string jsonData = "[";
        foreach(Jugador jug in listaJugadores)
        {
            jsonData += JsonUtility.ToJson(jug) + ",\n";
        }
        jsonData += "]";
        string path = "GameData.json";
        FileManager.WriteToFile(path, jsonData);
    }

    private void generarXML(List<Jugador> listaJugadores)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Jugador>));
        using (FileStream stream = new FileStream("GameData.xml", FileMode.Create))
        {
            serializer.Serialize(stream, listaJugadores);
        }
    }
}
