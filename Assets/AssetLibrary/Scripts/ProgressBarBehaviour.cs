using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarBehaviour : MonoBehaviour
{
    public GameObject progressBarBackground;
    public GameObject progressBarForeground;

    public float time = 0F;

    private float percentComplete;
    private float percentPerFrame;

    private bool hasStarted;

    public void StartProgressBar(float time)
    {
        this.time = time;
        hasStarted = true;
        percentPerFrame = (Time.fixedDeltaTime / time);
    }

    public void ResetProgressBar()
    {
        hasStarted = false;
        percentComplete = 0F;
    }

    void FixedUpdate()
    {
        if (hasStarted && percentComplete < 1F)
        {
            percentComplete += percentPerFrame;
            Vector3.Lerp(progressBarForeground.transform.position, new Vector3(0, 0, -1), percentPerFrame);
            Debug.Log(string.Format("{0}% done", percentComplete * 100));
        }
    }
}
