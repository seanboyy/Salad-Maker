using System;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private TextMesh text;

    public GameObject renderedCustomer;

    public ProgressBarBehaviour attachedBar;

    public Salad order;

    public Salad submittedFood = null;

    public GameObject powerUpPrefab;

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
        renderedCustomer.SetActive(true);
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
        renderedCustomer.SetActive(false);
        Debug.Log(string.Format("Customer is {0}satisfied and is now leaving", satisfied ? "" : "not "));
        if (attachedBar.percentComplete <= 0.3F)
        {
            if (RandomUtil.GenerateFloat() > 0.5F)
            {
                //Do spawn time up
                DoSpawnPowerUp(PowerUpType.TimeUp);
            }
            else
            {
                if (RandomUtil.GenerateFloat() > 0.5F)
                {
                    //Do spawn score up
                    DoSpawnPowerUp(PowerUpType.PointsUp);
                }
                else
                {
                    //Do spawn speed up
                    DoSpawnPowerUp(PowerUpType.SpeedUp);
                }
            }
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

    private void DoSpawnPowerUp(PowerUpType type)
    {
        var newPowerUp = Instantiate(powerUpPrefab);
        PowerUpBehaviour powerUp = newPowerUp.GetComponent<PowerUpBehaviour>();
        powerUp.type = type;
        powerUp.transform.position = RandomUtil.GenerateNonOccupiedPosition();
        powerUp.owningPlayer = submittingPlayer.playerNumber;
    }
}
