using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableDispenserBehaviour : MonoBehaviour
{
    public string vegetableType;
    // Start is called before the first frame update
    void Start()
    {
        TextMesh text = GetComponentInChildren<TextMesh>();
        text.text = vegetableType;
    }

    // Update is called once per frame
    void Update()
    { }
}
