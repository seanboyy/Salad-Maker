using System;
using System.Transactions;
using UnityEngine;

[Serializable]
public class Vegetable : ScoreObject
{

    [SerializeField]
    public bool isChopped;


    public Vegetable(string name = "") : base(name)
    {
        isChopped = false;
    }

    public Vegetable(Vegetable other)
    {
        name = other.name;
        isChopped = other.isChopped;
    }

    public void DoChop()
    {
        if (!isChopped)
        {
            isChopped = true;
            name = "Chopped " + name;
        }
    }
}
