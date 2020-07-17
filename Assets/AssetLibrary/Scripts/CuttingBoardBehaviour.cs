using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardBehaviour : MonoBehaviour
{
    private TextMesh text;
    public Vegetable heldItem;
    private ProgressBarBehaviour progressBar;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        progressBar = GetComponentInChildren<ProgressBarBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = heldItem.name;
        if (heldItem.name == "" && progressBar.hasStarted)
        {
            progressBar.ResetProgressBar();
        }
    }
}
