using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBehaviour : MonoBehaviour
{
    public ScoreObject heldObject = new ScoreObject();
    private TextMesh text;

    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
    }

    void Update()
    {
        //render plate text with the held object's name
        if (heldObject is Vegetable) text.text = heldObject.name;
        else text.text = heldObject.name.Replace(' ', '\n');
    }
}
