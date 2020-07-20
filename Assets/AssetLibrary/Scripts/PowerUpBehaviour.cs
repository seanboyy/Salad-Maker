using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    [SerializeField]
    public PowerUpType type;
    public int owningPlayer;
    public MainSceneController controller;
    private TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        controller = FindObjectOfType<MainSceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = type.ToString();
        switch (owningPlayer)
        {
            case 1:
                gameObject.GetComponent<Renderer>().material.color = GameConstants.Player1Color;
                break;
            case 2:
                gameObject.GetComponent<Renderer>().material.color = GameConstants.Player2Color;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour nearPlayer = other.gameObject.GetComponent<PlayerBehaviour>();
            if (nearPlayer.playerNumber == owningPlayer)
            {
                DoApplyPowerUp(nearPlayer);
                Destroy(gameObject);
            }
        }
    }

    void DoApplyPowerUp(PlayerBehaviour player)
    {
        switch (type)
        {
            case PowerUpType.PointsUp:
                switch (owningPlayer)
                {
                    case 1:
                        controller.player1Score += GameConstants.PointsUpRate;
                        break;
                    case 2:
                        controller.player2Score += GameConstants.PointsUpRate;
                        break;
                }
                break;
            case PowerUpType.SpeedUp:
                player.speed *= GameConstants.SpeedUpScalar;
                break;
            case PowerUpType.TimeUp:
                switch (owningPlayer)
                {
                    case 1:
                        controller.player1Time += GameConstants.TimeUpRate;
                        break;
                    case 2:
                        controller.player2Time += GameConstants.TimeUpRate;
                        break;
                }
                break;
        }
    }
}
