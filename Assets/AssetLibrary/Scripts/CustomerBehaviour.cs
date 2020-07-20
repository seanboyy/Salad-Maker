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
    public bool canAcceptFood = false;
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
        if (order != null) text.text = order.ToString().Replace(' ', '\n');
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
        canAcceptFood = true;
        renderedCustomer.SetActive(true);
        order = new Salad();
        var ingredients = RandomUtil.CreateRandomCombination();
        foreach (var ingredient in ingredients)
        {
            order.AddIngredient(ingredient);
        }
        if (attachedBar != null) attachedBar.StartProgressBar(GameConstants.WaitTime * GameConstants.RecipeComplexityScale * order.GetIngredientCount());
    }

    public void DoLeave(bool satisfied)
    {
        canAcceptFood = false;
        renderedCustomer.SetActive(false);
        var controller = FindObjectOfType<MainSceneController>();
        if (satisfied)
        {
            switch (submittingPlayer.playerNumber)
            {
                case 1:
                    controller.player1Score += GameConstants.ScorePerIngredient * order.GetIngredientCount();
                    break;
                case 2:
                    controller.player2Score += GameConstants.ScorePerIngredient * order.GetIngredientCount();
                    break;
            }
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
        }
        else
        {
            if (player1Mad && player2Mad)
            {
                controller.player1Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount() * 2;
                controller.player2Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount() * 2;
            }
            else if (player1Mad) controller.player1Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount() * 2;
            else if (player2Mad) controller.player2Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount() * 2;
            else
            {
                controller.player1Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount();
                controller.player2Score -= GameConstants.ScorePerIngredient * order.GetIngredientCount();
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

    public void ForceResetCustomer()
    {
        canAcceptFood = false;
        order = null;
        isActive = false;
        renderedCustomer.SetActive(false);
        player1Mad = false;
        player2Mad = false;
        attachedBar.ResetProgressBar();
    }
}
