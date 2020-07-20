using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//Code and game object structure adapted from 
//https://www.youtube.com/watch?v=iAbaqGYdnyI
public class HighScoreTableBehaviour : MonoBehaviour
{
    public Transform container;
    public Transform template;
    private readonly List<Transform> transformsList = new List<Transform>();

    public void DoUpdateHighscoreTable()
    {
        //destroy old highscore table if it exists
        foreach (var transform in transformsList) {
            Destroy(transform.gameObject);
        }
        //remove template (shouldn't show it)
        template.gameObject.SetActive(false);
        //grab the table from PlayerPrefs
        var jsonString = PlayerPrefs.GetString("highscoreTable");
        //decompose json into object
        var highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //this should only happen when the playerprefs fetch failed, but make a new object if it did
        if (highscores == null)
        {
            highscores = new Highscores();
        }
        //sort highscores table in descending order
        highscores.highscoreEntries = highscores.highscoreEntries.OrderByDescending(i => i.score).ToList();
        //display the minimum of either the count of highscores entries or 10
        for (int i = 0; i < Mathf.Min(highscores.highscoreEntries.Count, 10); ++i)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntries[i], container, transformsList);
        }
    }

    //Do adding new entry to highscore list
    public void AddHighscoreEntry(int score)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry(score);
        //fetch json from playerprefs
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        //decompose json into object
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        //if fetch failed, make a new object
        if (highscores == null)
        {
            highscores = new Highscores();
        }
        //add entry and then sort the list based on scores
        highscores.highscoreEntries.Add(highscoreEntry);
        highscores.highscoreEntries = highscores.highscoreEntries.OrderByDescending(i => i.score).ToList();
        //prune highscore list to only show top 10
        while (highscores.highscoreEntries.Count > 10)
        {
            highscores.highscoreEntries.RemoveAt(highscores.highscoreEntries.Count - 1);
        }
        //save new highscore table to playerprefs
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    //Do create gameobject for highscore entries
    private void CreateHighscoreEntryTransform(HighscoreEntry entry, Transform parent, List<Transform> transforms)
    {
        var height = 30F;
        Transform transform = Instantiate(template, parent);
        RectTransform rect = transform.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -height * transforms.Count);
        transform.gameObject.SetActive(true);
        var rank = transforms.Count + 1;
        //inline switch to convert number to ordinals
        string rankString = rank switch
        {
            1 => "1ST",
            2 => "2ND",
            3 => "3RD",
            _ => rank + "TH",
        };
        transform.Find("Position").GetComponent<Text>().text = rankString;
        transform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
        transforms.Add(transform);
    }

    #region DataClasses
    [Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntries;

        public Highscores()
        {
            highscoreEntries = new List<HighscoreEntry>();
        }
    }

    [Serializable]
    private class HighscoreEntry
    {
        public int score;

        public HighscoreEntry(int score)
        {
            this.score = score;
        }
    }
    #endregion
}
