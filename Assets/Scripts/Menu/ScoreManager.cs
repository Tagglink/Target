using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public struct Highscore {
    public string player_name { get; set; }
    public string map_name { get; set; }
    public float time { get; set; }
    public int dbIndex { get; set; } //corresponds to the database id
    public int index { get; set; } //corresponds to the in-script list

    public Highscore(string p, string m, float t, int dbi, int i) {
        player_name = p;
        map_name = m;
        time = t;
        dbIndex = dbi;
        index = i;
    }
}

public class ScoreManager : MonoBehaviour {

    public GameObject timeText;
    public GameObject highscorePrefab;
    public GameObject scrollBarObject;
    
    Scrollbar scrollBar;
    GameObject selectedHighscore;

    public int highscoreStartY = 275;
    public int visibleHighscores = 12;
    public int highscoreHeightMargin = 50;

    List<GameObject> highscoreEntries;
    List<Highscore> sortedHighscores;

    IDbConnection dbConnection;

    void Start()
    {
        ConnectToHighscoreDB();

        highscoreEntries = new List<GameObject>();
        sortedHighscores = GetHighscoreTable().Where(o => o.map_name == "Temple").OrderBy(o => o.time).ToList();
        LoadHighscoresToUI();

        scrollBar = scrollBarObject.GetComponent<Scrollbar>();

        AdaptScrollbarToList();

        Debug.Log(scrollBar.handleRect.ToString());
    }

    void FixedUpdate()
    {
        Vector2 pos = new Vector2(0, 0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollBar.handleRect, Camera.main.WorldToScreenPoint(Input.mousePosition), Camera.main, out pos);
        bool scrollReady = RectTransformUtility.RectangleContainsScreenPoint(scrollBar.handleRect, Input.mousePosition, Camera.main);
        Debug.Log(Input.mousePosition);
        Debug.Log(scrollReady.ToString());

        if (scrollReady)
            scrollBar.value += Input.mouseScrollDelta.y;
    }

    void OnApplicationQuit()
    {
        dbConnection.Close();
        dbConnection = null;
    }

    public void MoveHighscorePrefabs()
    {
        float highscoreTransLen = float.Parse(highscoreEntries.Count.ToString());

        float value = scrollBar.value;
        float highscoreListHeight = highscoreHeightMargin * (highscoreEntries.Count - visibleHighscores);
        float mod = (int)(value * highscoreListHeight);

        for (int i = 0; i < highscoreTransLen; i++)
        {
            float start = highscoreStartY - (i * highscoreHeightMargin);
            highscoreEntries[i].GetComponent<RectTransform>().localPosition = new Vector3(0, start + mod, 0);
        }
    }

    public void AdaptScrollbarToList()
    {
        if (highscoreEntries.Count > visibleHighscores)
        {
            scrollBar.gameObject.SetActive(true);
            scrollBar.size = 1.0f / (highscoreEntries.Count - visibleHighscores + 1.0f);
        }
        else
            scrollBar.gameObject.SetActive(false);
    }

    public void LoadHighscoresToUI()
    {
        for (int i = 0; i < sortedHighscores.Count; i++)
        {
            GameObject newHighscorePrefab = Instantiate(highscorePrefab, gameObject.transform, false) as GameObject;
            newHighscorePrefab.GetComponent<Button>().onClick.AddListener(() => SelectHighscore(newHighscorePrefab));
            newHighscorePrefab.GetComponent<RectTransform>().localPosition = new Vector3(0, highscoreStartY - (i * highscoreHeightMargin), 0);
            int originalIndex = sortedHighscores[i].index;
            int databaseID = sortedHighscores[i].dbIndex;
            int sortedIndex = i;
            newHighscorePrefab.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => RemoveHighscore(databaseID, originalIndex, sortedIndex));

            Text[] textObjects = newHighscorePrefab.GetComponentsInChildren<Text>();

            float time = sortedHighscores[i].time;
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;
            int decimals = (int)((time - Mathf.Floor(time)) * 100);

            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, decimals);

            textObjects[3].text = "#" + (i + 1);
            textObjects[1].text = formattedTime;
            textObjects[0].text = sortedHighscores[i].player_name;

            highscoreEntries.Add(newHighscorePrefab);
        }
    }

    void AssignRemoveHighscoreListeners()
    {
        for (int i = 0; i < highscoreEntries.Count; i++)
        {
            int sortedIndex = i;
            int databaseID = sortedHighscores[i].dbIndex;
            int originalIndex = sortedHighscores[i].index;
            highscoreEntries[i].GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
            highscoreEntries[i].GetComponentsInChildren<Button>()[1].onClick.AddListener(() => RemoveHighscore(databaseID, originalIndex, sortedIndex));
        }
    }

    public void ConnectToHighscoreDB()
    {
        string connectionString = "URI=file:" + Application.dataPath + "/Highscores.s3db";
        dbConnection = (IDbConnection)new SqliteConnection(connectionString);
        dbConnection.Open(); //Open connection to the database.
    }

    public void CleanseUIHighscores()
    {
        List<GameObject> arrayCopy = highscoreEntries;
        foreach (GameObject h in arrayCopy)
        {
            Destroy(h);
        }
        highscoreEntries = new List<GameObject>();
    }

    void UpdateHighscoreRankings()
    {
        for (int i = 0; i < sortedHighscores.Count; i++)
        {
            highscoreEntries[i].GetComponentsInChildren<Text>()[3].text = "#" + (i + 1);

            Highscore updatedHighscore = sortedHighscores[i];
            updatedHighscore.index = i;
            sortedHighscores[i] = updatedHighscore;
        }
    }

    void UpdateListChanges()
    {
        MoveHighscorePrefabs();
        AssignRemoveHighscoreListeners();
        UpdateHighscoreRankings();
        AdaptScrollbarToList();
    }

    public List<Highscore> GetHighscoreTable()
    {
        List<Highscore> recieverList = new List<Highscore>();

        string sqlQuery = "SELECT player_id, player_name, time, map_name FROM Highscores";
        IDbCommand dbcmd = dbConnection.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        int i = 0;
        while (reader.Read())
        {
            int player_id = reader.GetInt32(0);
            string playerName = reader.GetString(1);
            float time = reader.GetFloat(2);
            string mapName = reader.GetString(3);

            recieverList.Add(new Highscore(playerName, mapName, time, player_id, i));

            Debug.Log("playerName=" + playerName + " time=" + time + " mapName=" + mapName + " played_id=" + player_id);
            i++;
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;

        return recieverList;
    }

    public void RemoveHighscore(int databaseID, int origIndex, int sortedIndex)
    {
        string sqlQuery = "DELETE FROM Highscores WHERE player_id=" + databaseID;
        IDbCommand dbcmd = dbConnection.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;

        Destroy(selectedHighscore);
        highscoreEntries.RemoveAt(sortedIndex);
        sortedHighscores.RemoveAt(sortedIndex);

        UpdateListChanges();
    }

    public void CleanseHighscoresInTheNameOfGabrielReyes()
    {
        //DIE, DIE, DIE HIGHSCORES!
        //POTG
        //Gabriel Reyes
        //As Reaper
        string sqlQuery = "DELETE FROM Highscores";
        IDbCommand dbcmd = dbConnection.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;

        HardRefresh();
    }

    void RefreshHighscorePositions()
    {
        for (int i = 0; i < highscoreEntries.Count; i++)
            highscoreEntries[i].GetComponent<RectTransform>().localPosition = new Vector3(0, highscoreStartY - (i * highscoreHeightMargin), 0);
    }

    void HardRefresh()
    {
        CleanseUIHighscores();

        sortedHighscores = GetHighscoreTable().Where(o => o.map_name == "Temple").OrderBy(o => o.time).ToList();

        LoadHighscoresToUI();

        UpdateListChanges();
    }

    // For Removing
    public void SelectHighscore(GameObject highscoreEntry)
    {
        bool isInteractable = highscoreEntry.GetComponentsInChildren<Button>()[1].interactable;

        if (isInteractable)
        {
            highscoreEntry.GetComponentsInChildren<Button>()[1].interactable = false;
        }
        else
            highscoreEntry.GetComponentsInChildren<Button>()[1].interactable = true;
        
        //un-highlight the last x-button
        if (selectedHighscore != null && selectedHighscore != highscoreEntry)
            selectedHighscore.GetComponentsInChildren<Button>()[1].interactable = false;

        //switch the selected entry
        selectedHighscore = highscoreEntry;
    }
}
