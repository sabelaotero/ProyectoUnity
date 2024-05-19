using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    public DBManager manager;
    public TMP_InputField inputName;
    public string nombreJugadorInput;
    public int jugadorID;

    public void iniciarJuego()
    {
        SceneManager.LoadScene(1);
    }

    public void NuevoJugadorClick()
    {
        nombreJugadorInput = inputName.text;
        bool e = manager.ExisteNombre(nombreJugadorInput); // No existe --> e = False
        bool nohayJug = false;
        if (!string.IsNullOrEmpty(nombreJugadorInput) && e == nohayJug) //Si no existe un jugador con ese nombre --> el NUEVO se puede llamar asi por lo que lo añado
        {
            manager.AñadirJugador(nombreJugadorInput);
            jugadorID = manager.GetIdByName(nombreJugadorInput);
            manager.AñadirPart(jugadorID);
            iniciarJuego();
        }
        else
        {
            Debug.LogWarning("Nombre no válido, ingresa otro nombre");
        }
    }

    public void JugadorExistenteClick()
    {
        nombreJugadorInput = inputName.text;
        bool e = manager.ExisteNombre(nombreJugadorInput);
        bool hayJug = true;
        if (e == hayJug) //Si existe un jugador con ese nombre --> podra iniciar el juego
        {
            jugadorID = manager.GetIdByName(nombreJugadorInput);
            manager.AñadirPart(jugadorID);
            iniciarJuego();
        }
        else
        {
            Debug.LogWarning("Jugador no encontrado, ingrese un nombre existente o haz click en 'Nuevo Jugador'");
        }
    }

    public void ElimJugador()
    {
        nombreJugadorInput = inputName.text;
        bool e = manager.ExisteNombre(nombreJugadorInput);
        bool hayJug = true;
        if (e == hayJug) //Si existe un jugador con ese nombre --> borrarlo
        {
            jugadorID = manager.GetIdByName(nombreJugadorInput);
            manager.EliminarJugador(jugadorID);
            Debug.Log("Jugador eliminado con éxito");
        }
        else
        {
            Debug.LogWarning("Jugador no encontrado, no se ha podido eliminar el jugador");
        }
    }
}
