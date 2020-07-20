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

    public override bool Equals(object obj)
    {
        return obj is Vegetable vegetable && vegetable.name == name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}
