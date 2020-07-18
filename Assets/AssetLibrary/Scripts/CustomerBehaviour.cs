using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class CustomerBehaviour : MonoBehaviour
{
    private TextMesh text;

    public GameObject renderedCustomer;

    public ProgressBarBehaviour attachedBar;

    public Salad order;

    public Salad submittedFood = null;

    [NonSerialized]
    public PlayerBehaviour submittingPlayer = null;

    public bool isActive = false;
    public bool player1Mad = false;
    public bool player2Mad = false;

    private Vector3 position;

    void Start()
    {
        renderedCustomer.SetActive(false);
        text = GetComponentInChildren<TextMesh>();
        position = transform.position;
    }


    void Update()
    {
        transform.position = position;
        if (isActive) renderedCustomer.SetActive(true);
        else renderedCustomer.SetActive(false);
        if (order != null) text.text = order.ToString().Replace(' ','\n');
        else text.text = "";
        if (submittedFood != null)
        {
            if (order.CompareTo(submittedFood)) DoLeave(true);
            else GetAngry();
            submittingPlayer = null;
            submittedFood = null;
        }
        if (attachedBar.isDone) DoLeave(false);
    }

    public void StartCustomer()
    {
        Debug.Log("Starting Customer");
        order = new Salad();
        var ingredients = RandomUtil.CreateRandomCombination();
        foreach (var ingredient in ingredients)
        {
            order.AddIngredient(ingredient);
        }
        Debug.Log(order);
        if (attachedBar != null) attachedBar.StartProgressBar(GameConstants.WaitTime * GameConstants.RecipeComplexityScale * order.GetIngredientCount());
    }

    public void DoLeave(bool satisfied)
    {
        Debug.Log(string.Format("Customer is {0}satisfied and is now leaving", satisfied ? "" : "not "));
        if (attachedBar.percentComplete <= 0.3F)
        {
            //Do spawn powerup
        }
        order = null;
        isActive = false;
        player1Mad = false;
        player2Mad = false;
        attachedBar.ResetProgressBar();
    }

    public void GetAngry()
    {
        attachedBar.DoAngryCountdownStep();
        Debug.Log("Incorrect, Customer is now Angry");
        switch (submittingPlayer.playerNumber)
        {
            case 1:
                player1Mad = true;
                break;
            case 2:
                player2Mad = true;
                break;
        }
    }
}
