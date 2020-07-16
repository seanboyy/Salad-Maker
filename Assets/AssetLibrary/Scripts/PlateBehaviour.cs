using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBehaviour : MonoBehaviour
{
    public string heldVegetable = "";
    private TextMesh text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = heldVegetable;
    }
}
