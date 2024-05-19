using UnityEngine;
using System;

[Serializable]
public class Jugador 
{
    //public int id;
    public string nombre;
    public int maxPuntos;
    public int numPartidas;

    public Jugador()
    {
    }

    public Jugador(string nombre, int puntos)
    {
        this.nombre = nombre;
        this.maxPuntos = puntos;
    }

    public Jugador(string nombre, int puntos, int partidas)
    {
        this.nombre = nombre;
        this.numPartidas = partidas;
        this.maxPuntos = puntos;
    }


    public void AñadirPart(int partidas)
    {
        this.numPartidas = partidas;
    }

    public void print()
    {
        Debug.Log("Nombre: " + nombre + ", Puntuación mas alta: " + maxPuntos + ", Partidas jugadas: " + numPartidas + "\n");
    }
}