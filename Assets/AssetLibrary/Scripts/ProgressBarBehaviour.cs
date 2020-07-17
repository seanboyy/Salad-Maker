using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProgressBarBehaviour : MonoBehaviour
{
    public GameObject progressBarBackground;
    public GameObject progressBarForeground;

    public float time = 0F;

    private float percentComplete;
    private float percentPerFrame;
    private float positionMoveScalar = 0F;
    private float scaleGrowScalar = 0F;

    public bool hasStarted;
    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = progressBarForeground.transform.position;
        startScale = progressBarForeground.transform.localScale;
        progressBarBackground.SetActive(false);
    }

    public void StartProgressBar(float time)
    {
        progressBarBackground.SetActive(true);
        this.time = time;
        hasStarted = true;
        percentPerFrame = (Time.fixedDeltaTime / time);
        positionMoveScalar = 0.5F * percentPerFrame;
        scaleGrowScalar = percentPerFrame;
    }

    public void ResetProgressBar()
    {
        progressBarBackground.SetActive(false);
        hasStarted = false;
        percentComplete = 0F;
        progressBarForeground.transform.position = startPos;
        progressBarForeground.transform.localScale = startScale;
    }

    void FixedUpdate()
    {
        if (hasStarted && percentComplete < 1F)
        {
            percentComplete += percentPerFrame;
            progressBarForeground.transform.position = new Vector3(progressBarForeground.transform.position.x + positionMoveScalar, progressBarForeground.transform.position.y, progressBarForeground.transform.position.z);
            progressBarForeground.transform.localScale = new Vector3(progressBarForeground.transform.localScale.x + scaleGrowScalar, progressBarForeground.transform.localScale.y, progressBarForeground.transform.localScale.z);
            Debug.Log(string.Format("{0}% done", percentComplete * 100));
        }
    }
}
