using System;
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

    public bool isReversed = false;

    [NonSerialized]
    public bool hasStarted;
    public bool isDone = false;
    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        if (!isReversed)
        {
            startPos = progressBarForeground.transform.position;
            startScale = progressBarForeground.transform.localScale;
        }
        else
        {
            progressBarForeground.transform.position = new Vector3(progressBarForeground.transform.position.x + 0.5F, progressBarForeground.transform.position.y, progressBarForeground.transform.position.z);
            startPos = progressBarForeground.transform.position;
            progressBarForeground.transform.localScale = new Vector3(progressBarForeground.transform.localScale.x + 1F, progressBarForeground.transform.localScale.y, progressBarForeground.transform.localScale.z);
            startScale = progressBarForeground.transform.localScale;
            progressBarForeground.SetActive(false);
        }
        progressBarBackground.SetActive(false);
        Debug.Log(startPos);
        Debug.Log(startScale);
    }

    public void StartProgressBar(float time)
    {
        isDone = false;
        progressBarBackground.SetActive(true);
        this.time = time;
        hasStarted = true;
        percentPerFrame = (Time.fixedDeltaTime / time);
        if (isReversed)
        {
            progressBarForeground.SetActive(true);
            positionMoveScalar = -0.5F * percentPerFrame;
            scaleGrowScalar = -percentPerFrame;
        }
        else
        {
            positionMoveScalar = 0.5F * percentPerFrame;
            scaleGrowScalar = percentPerFrame;
        }
    }

    public void ResetProgressBar()
    {
        isDone = false;
        progressBarBackground.SetActive(false);
        hasStarted = false;
        percentComplete = 0F;
        progressBarForeground.transform.position = startPos;
        progressBarForeground.transform.localScale = startScale;
        if (isReversed) progressBarForeground.SetActive(false);
    }

    void FixedUpdate()
    {
        if (hasStarted && percentComplete < 1F)
        {
            percentComplete += percentPerFrame;
            progressBarForeground.transform.position += new Vector3(positionMoveScalar, 0, 0);
            progressBarForeground.transform.localScale += new Vector3(scaleGrowScalar, 0, 0);
        }
        else if (percentComplete >= 1F)
        {
            isDone = true;
        }
    }

    public void DoAngryCountdownStep()
    {
        percentPerFrame *= GameConstants.AngrySpeedupFactor;
        scaleGrowScalar *= GameConstants.AngrySpeedupFactor;
        positionMoveScalar *= GameConstants.AngrySpeedupFactor;
    }
}
