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
        foreach (var transform in transformsList) {
            Destroy(transform.gameObject);
        }
        template.gameObject.SetActive(false);
        var jsonString = PlayerPrefs.GetString("highscoreTable");
        Debug.Log(jsonString);
        var highscores = JsonUtility.FromJson<Highscores>(jsonString);
        if (highscores == null)
        {
            highscores = new Highscores();
        }
        highscores.highscoreEntries = highscores.highscoreEntries.OrderByDescending(i => i.score).ToList();
        for (int i = 0; i < Mathf.Min(highscores.highscoreEntries.Count, 10); ++i)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntries[i], container, transformsList);
        }
    }

    public void AddHighscoreEntry(int score)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry(score);
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        if (highscores == null)
        {
            highscores = new Highscores();
        }
        highscores.highscoreEntries.Add(highscoreEntry);
        highscores.highscoreEntries = highscores.highscoreEntries.OrderByDescending(i => i.score).ToList();
        while (highscores.highscoreEntries.Count > 10)
        {
            highscores.highscoreEntries.RemoveAt(highscores.highscoreEntries.Count - 1);
        }
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry entry, Transform parent, List<Transform> transforms)
    {
        var height = 30F;
        Transform transform = Instantiate(template, parent);
        RectTransform rect = transform.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -height * transforms.Count);
        transform.gameObject.SetActive(true);
        var rank = transforms.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
            default:
                rankString = rank + "TH";
                break;
        }
        transform.Find("Position").GetComponent<Text>().text = rankString;
        transform.Find("Score").GetComponent<Text>().text = entry.score.ToString();
        transforms.Add(transform);
    }

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
}
