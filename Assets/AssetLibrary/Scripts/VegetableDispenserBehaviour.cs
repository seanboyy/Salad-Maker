using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableDispenserBehaviour : MonoBehaviour
{
    public Vegetable vegetableType;
    // Start is called before the first frame update
    void Start()
    {
        TextMesh text = GetComponentInChildren<TextMesh>();
        text.text = vegetableType.name;
    }
}
