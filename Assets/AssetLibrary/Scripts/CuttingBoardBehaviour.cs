using System;
using System.Text;
using UnityEngine;

public class CuttingBoardBehaviour : MonoBehaviour
{
    private TextMesh text;
    public Vegetable ingredient;
    public Salad activeSalad = null;
    private ProgressBarBehaviour progressBar;
    public bool working = false;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        progressBar = GetComponentInChildren<ProgressBarBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        StringBuilder stringBuilder = new StringBuilder(ingredient != null ? ingredient.name : "");
        stringBuilder.Append("\n" + (activeSalad != null ? activeSalad.name : ""));
        text.text = stringBuilder.ToString();
        if (ingredient.name == "" && progressBar.hasStarted)
        {
            progressBar.ResetProgressBar();
        }
    }

    public void CreateSaladFromIngredient()
    {
        if (activeSalad == null)
        {
            activeSalad = new Salad();
        }
        activeSalad.AddIngredient(ingredient);
        ingredient = new Vegetable();
    }
}
