using System;
using UnityEngine;

[Serializable]
public class Vegetable
{
    [SerializeField]
    public string name;

    [SerializeField]
    public bool isChopped;


    public Vegetable(string name = "")
    {
        this.name = name;
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
