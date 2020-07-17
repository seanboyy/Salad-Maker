using System;
using UnityEngine;

[Serializable]
public class ScoreObject
{
    [SerializeField]
    public string name;

    public ScoreObject(string name = "")
    {
        this.name = name;
    }
}
