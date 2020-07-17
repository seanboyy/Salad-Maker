using UnityEngine;

public class CuttingBoardBehaviour : MonoBehaviour
{
    private TextMesh text;
    public Vegetable ingredient;
    public Salad activeSalad;
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
        text.text = ingredient.name;
        if (ingredient.name == "" && progressBar.hasStarted)
        {
            progressBar.ResetProgressBar();
        }
    }
}
