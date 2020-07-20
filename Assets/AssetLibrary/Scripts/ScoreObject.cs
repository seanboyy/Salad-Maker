using System;
using UnityEngine;

//parent class of salad and vegetable, allows plate and cuttingboard to hold either 
[Serializable]
public class ScoreObject
{
    [SerializeField]
    public string name;

    public ScoreObject(string name = "")
    {
        this.name = name;
    }

    public override string ToString()
    {
        return name;
    }
}
