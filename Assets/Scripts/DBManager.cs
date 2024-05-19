using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class DBManager : MonoBehaviour
{
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_CREATE_JUGADORES = "CREATE TABLE IF NOT EXISTS Jugadores (Id INTEGER UNIQUE NOT NULL PRIMARY KEY," +
        "                                                                        Nombre TEXT UNIQUE, " +
        "                                                                        MaxPunt INTEGER DEFAULT 0);";
    private string SQL_CREATE_SESIONES = "CREATE TABLE IF NOT EXISTS Sesiones (Id INTEGER UNIQUE NOT NULL PRIMARY KEY, " +
        "                                                                      JugadorId INTEGER, " +
        "                                                                      Punt INTEGER DEFAULT 0, " +
        "                                                                      Fecha DATE," +
        "                                                                      FOREIGN KEY (JugadorId) REFERENCES Jugadores(Id) ON DELETE CASCADE);";
    private string SQL_CREATE_RECOMPENSAS = "CREATE TABLE IF NOT EXISTS Recompensas (Id INTEGER UNIQUE NOT NULL PRIMARY KEY, " +
        "                                                                           SesionId INTEGER, " +
        "                                                                           Prenda TEXT NOT NULL, " +
        "                                                                           Color TEXT NOT NULL," +
        "                                                                           Monedas INTEGER NOT NULL CHECK (Monedas BETWEEN 0 AND 15), " +
        "                                                                           FOREIGN KEY (SesionId) REFERENCES Sesiones(Id) ON DELETE CASCADE);";
    private string[] prendas = { "Camiseta", "Gorro de vaquero", "Gorra", "Sombrero de playa", "Capa" };
    private string[] colores = { "Azul", "Blanco", "Rojo", "Verde", "Amarillo", "Morado", "Rosa", "Naranja" };
    private IDbConnection dbConnection;

    // Start is called before the first frame update
    void Start()
    {
        dbConnection = CreateAndOpenDataBase();
    }


    public void CerrarDB()
    {
        dbConnection.Close();
    }

    private IDbConnection CreateAndOpenDataBase()
    {

        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();


        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_JUGADORES;
        dbCmd.ExecuteReader();
        dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_SESIONES;
        dbCmd.ExecuteReader();
        dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_RECOMPENSAS;
        dbCmd.ExecuteReader();

        return dbConnection;
    }

    public void AñadirPuntos(int punt)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        int ult = IdUltPart();
        dbCmd.CommandText = $"UPDATE Sesiones SET Punt = '{punt}' WHERE Id = (SELECT MAX(Id) FROM Sesiones);";
        IDataReader reader = dbCmd.ExecuteReader();

        actualizarMaxpunt(punt);
    }

    private void actualizarMaxpunt(int punt)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT MaxPunt FROM Jugadores WHERE Id = (SELECT JugadorId FROM Sesiones WHERE Id = (SELECT MAX(Id) FROM Sesiones));";
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int maxPunt = reader.GetInt32(0);
        reader.Close();
        if(punt > maxPunt)
        {
            dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = $"UPDATE Jugadores SET MaxPunt = '{punt}' WHERE Id = (SELECT JugadorId FROM Sesiones WHERE Id = (SELECT MAX(Id) FROM Sesiones));";
            reader = dbCmd.ExecuteReader();
        }
    }

    public int IdJugadorUltPart()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT Id FROM Jugadores WHERE Id = (SELECT JugadorId FROM Sesiones WHERE Id = (SELECT MAX(Id) FROM Sesiones));";
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int jugId = reader.GetInt32(0);
        reader.Close();
        return jugId;
    }

    public void MostrarRank()
    {
        int numJug = countJugadores();
        if (numJug > 0)
        {

            List<Jugador> jugadores = new List<Jugador>();
            IDbCommand dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = $"SELECT Nombre, MaxPunt FROM Jugadores ORDER BY MaxPunt DESC LIMIT 5;";
            IDataReader reader = dbCmd.ExecuteReader();
            while (reader.Read())
            {
                string nombre = reader.GetString(0);
                int puntos = reader.GetInt32(1);
                Jugador jugNew = new Jugador(nombre, puntos);
                jugadores.Add(jugNew);
            }

            foreach (Jugador jugador in jugadores)
            {
                dbCmd = dbConnection.CreateCommand();
                dbCmd.CommandText = $"SELECT count(*) FROM Sesiones WHERE  JugadorId = (SELECT Id FROM Jugadores WHERE Nombre = '{jugador.nombre}');";
                reader = dbCmd.ExecuteReader();
                reader.Read();
                int part = reader.GetInt32(0);
                jugador.AñadirPart(part);
            }
            reader.Close();

            foreach (Jugador jugador in jugadores)
            {
                jugador.print();
            }
        }
        else
        {
            Debug.Log("Todavia no hay ningún jugador guardado");
        }
    }

    private int countJugadores()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT count(*) FROM Jugadores;";
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int numJug = reader.GetInt32(0);
        reader.Close();
        return numJug;
    }

    internal bool ExisteNombre(string nombreJugador)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT Nombre FROM Jugadores WHERE REPLACE(Nombre, ' ', '')= REPLACE('{nombreJugador}', ' ', '');";
        IDataReader reader = dbCmd.ExecuteReader();
        bool existe;
        if (!reader.Read())
        {
            existe = false;
            reader.Close();
            return (existe);
        }
        else
        {
            existe = true;
            reader.Close();
            return (existe);
        }
    }

    internal void AñadirJugador(string playerName)
    {
        string command = $"INSERT INTO Jugadores (Nombre) VALUES ('{playerName}');";
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = command;
        dbCmd.ExecuteNonQuery();
    }

    internal void AñadirPart(int jugId)
    {
        DateTime fecha = DateTime.Now;
        string fechaSQL = fecha.ToString("yyyy-MM-dd");
        string command = $"INSERT INTO Sesiones (JugadorId, Fecha) VALUES ('{jugId}', '{fechaSQL}')";
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = command;
        dbCmd.ExecuteNonQuery();
        crearRandomRecomp();
    }

    private int IdUltPart()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT MAX(Id) FROM Sesiones;";
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        int sesionId = reader.GetInt32(0);
        reader.Close();
        return sesionId;
    }


    internal void EliminarJugador(int jugId)
    {
        string command = $"DELETE FROM Jugadores WHERE Id = '{jugId}';";
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = command;
        dbCmd.ExecuteNonQuery();
    }

    internal int GetIdByName(string nombre)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = $"SELECT Id FROM Jugadores WHERE REPLACE(Nombre, ' ', '')= REPLACE('{nombre}', ' ', '');";
        IDataReader reader = dbCmd.ExecuteReader();
        reader.Read();
        return reader.GetInt32(0);
    }

    public void crearRandomRecomp()
    {
        int sesId = IdUltPart();

        IDbCommand dbCmd = dbConnection.CreateCommand();
        string comandoRec = $"INSERT INTO Recompensas (SesionId, Prenda, Color, Monedas) VALUES ";
        System.Random rnd = new System.Random();
        string prenda = prendas[rnd.Next(prendas.Length)];
        string color = colores[rnd.Next(colores.Length)];
        int monedas = UnityEngine.Random.Range(0, 16);
        comandoRec += $"('{sesId}', '{prenda}', '{color}', '{monedas}')";
        dbCmd.CommandText = comandoRec;
        dbCmd.ExecuteNonQuery();
    }

    public List<Jugador> TodosJug()
    {
        int numJug = countJugadores();
        if (numJug > 0)
        {

            List<Jugador> jugadores = new List<Jugador>();
            IDbCommand dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = $"SELECT Nombre, MaxPunt FROM Jugadores ORDER BY MaxPunt DESC;";
            IDataReader reader = dbCmd.ExecuteReader();
            while (reader.Read())
            {
                string nombre = reader.GetString(0);
                int puntos = reader.GetInt32(1);
                Jugador jugNew = new Jugador(nombre, puntos);
                jugadores.Add(jugNew);
            }

            foreach (Jugador jugador in jugadores)
            {
                dbCmd = dbConnection.CreateCommand();
                dbCmd.CommandText = $"SELECT count(*) FROM Sesiones WHERE  JugadorId = (SELECT Id FROM Jugadores WHERE Nombre = '{jugador.nombre}');";
                reader = dbCmd.ExecuteReader();
                reader.Read();
                int part = reader.GetInt32(0);
                jugador.AñadirPart(part);
            }
            reader.Close();

            return jugadores;
        }
        else
        {
            Debug.Log("Todavia no hay ningún jugador guardado");
            return null;
        }
    }

}
