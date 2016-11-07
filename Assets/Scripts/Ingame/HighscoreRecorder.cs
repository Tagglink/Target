using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

public class HighscoreRecorder : MonoBehaviour {

    public GameObject timeText;
    public string tempName;
    public bool balloonsPopped;

    // Use this for initialization
    void Start () {
        balloonsPopped = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (!balloonsPopped)
            CheckBalloons();
    }

    void CheckBalloons()
    {
        if (GameObject.FindGameObjectsWithTag("Balloon").Length == 0)
            Complete();
        timeText.GetComponent<Text>().text = Math.Round(Time.timeSinceLevelLoad, 1).ToString();
    }

    // Is called when all balloons have been popped
    void Complete()
    {
        balloonsPopped = true;
        float endTime = Time.timeSinceLevelLoad;
        PostHighscore(tempName, endTime, "Temple");
        Debug.Log("All balloons have been popped as of: " + endTime.ToString());
        double showTime = Math.Round(Time.timeSinceLevelLoad, 1);
    }

    public void PostHighscore(string player_name, float time, string map_name)
    {
        string sqlQuery = "INSERT INTO Highscores (player_name, time, map_name) VALUES ('" + player_name + "', '" + time + "', '" + map_name + "')";
        string connectionString = "URI=file:" + Application.dataPath + "/Highscores.s3db";
        IDbConnection dbConnection;
        dbConnection = (IDbConnection)new SqliteConnection(connectionString);
        dbConnection.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbConnection.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbConnection.Close();
        dbConnection = null;
    }
}
