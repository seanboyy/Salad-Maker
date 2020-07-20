using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    public List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    [SerializeField]
    public List<Vector3> dispenserPositions = new List<Vector3>();

    [Header("Texts")]
    public GameObject Player1Text;
    public GameObject Player2Text;
    [Header("Prefabs")]
    public GameObject DispensingStation;
    [Header("Walls")]
    public GameObject TopWall;
    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject BottomWall;
    [Header("Counters")]
    public GameObject TopCounter;
    public GameObject BottomCounter;
    public GameObject LeftCounter;
    public GameObject RightCounter;
    [Header("Timers")]
    public int player1Time = 0;
    public int player2Time = 0;
    [Header("Scores")]
    public int player1Score = 0;
    public int player2Score = 0;
    [Header("Players")]
    public PlayerBehaviour player1;
    public PlayerBehaviour player2;
    [Header("Info Screens")]
    public GameObject EndGameScreen;
    public GameObject StartGameScreen;

    private float aspect = 0.0F;

    private bool secondPass = false;
    private bool gameStart = false;
    private bool gameOver = false;
    private bool gameIsStopped = false;

    void Start()
    {
        EndGameScreen.SetActive(false);
        player1Time = 120;
        player2Time = 120;
        //set playable boundary area relative to camera extent
        float extent = Camera.main.orthographicSize;
        aspect = Camera.main.aspect;
        float halfHeight = extent;
        float halfWidth = aspect * extent;
        float minX = -halfWidth;
        float maxX = halfWidth;
        float minY = -halfHeight;
        float maxY = halfHeight;
        TopWall.transform.position = new Vector3(0, maxY + 0.5F, 0);
        BottomWall.transform.position = new Vector3(0, minY - 0.5F, 0);
        LeftWall.transform.position = new Vector3(minX - 0.5F, 0, 0);
        RightWall.transform.position = new Vector3(maxX + 0.5F, 0, 0);
        TopCounter.transform.position = new Vector3(0, maxY - 1.5F, -1.25F);
        BottomCounter.transform.position = new Vector3(0, minY + 0.5F, -1.5F);
        RightCounter.transform.position = new Vector3(maxX - 0.5F, 0, -1.5F);
        LeftCounter.transform.position = new Vector3(minX + 0.5F, 0, -1.5F);
        Player2Text.transform.position = new Vector3(maxX - 1.5F, maxY - 0.5F, -2F);
        Player1Text.transform.position = new Vector3(minX + 1.5F, maxY - 0.5F, -2F);
        minY += 1;
        //set dispenser positions relative to camera extent
        dispenserPositions.Add(new Vector3(minX + 0.5F, 0, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, 0, -2));
        dispenserPositions.Add(new Vector3(minX + 0.5F, minY * 1 / 3F, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, minY * 1 / 3F, -2));
        dispenserPositions.Add(new Vector3(minX + 0.5F, minY * 2 / 3F, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, minY * 2 / 3F, -2));
        //shuffle list of vegetables to randomise positions in dispensers
        var vegetables = new List<Vegetable>(GameConstants.Vegetables);
        RandomUtil.Shuffle(vegetables);
        for (int i = 0; i < vegetables.Count; ++i)
        {
            GameObject dispenser = Instantiate(DispensingStation);
            dispenser.transform.position = dispenserPositions[i];
            dispenser.GetComponent<VegetableDispenserBehaviour>().vegetableType = vegetables[i];
        }
    }

    void Update()
    {
        //set player timeout to make them unable to move when time is up
        if (player1Time == 0) player1.timeIsUp = true;
        if (player2Time == 0) player2.timeIsUp = true;
        //render player info text (score and time remaining)
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder1.Append("Time: " + player1Time);
        stringBuilder1.Append("\nScore: " + player1Score);
        stringBuilder2.Append("Time: " + player2Time);
        stringBuilder2.Append("\nScore: " + player2Score);
        Player1Text.GetComponent<TextMesh>().text = stringBuilder1.ToString();
        Player2Text.GetComponent<TextMesh>().text = stringBuilder2.ToString();
        //a second has passed, timer coroutine is over and game has not stopped, start another second countdown
        if (secondPass && !gameOver) StartCoroutine("Timer");
        //game over! Stop the game
        if (player1Time == 0 && player2Time == 0 && !gameIsStopped)
        {
            DoStopGame();
        }
        //activate customers whenever they are inactive
        if (!gameOver && gameStart)
        {
            customers.ForEach(customer =>
            {
                if (!customer.isActive) StartCoroutine("WaitToStartCustomer", customer);
            });
        }
    }

    //game over! 
    void DoStopGame()
    {
        gameIsStopped = true;
        StopAllCoroutines();
        //stop the customers
        foreach (var customer in customers)
        {
            customer.isActive = false;
        }
        //render endgamescreen and startup highscore table
        EndGameScreen.SetActive(true);
        HighScoreTableBehaviour highScoreTable = EndGameScreen.GetComponentInChildren<HighScoreTableBehaviour>();
        Debug.Log(highScoreTable.name);
        //save highest score from game or just P1 if they are both equal
        if (player1Score != player2Score)
        {
            highScoreTable.AddHighscoreEntry(Mathf.Max(player1Score, player2Score));
        }
        else
        {
            highScoreTable.AddHighscoreEntry(player1Score);
        }
        highScoreTable.DoUpdateHighscoreTable();
    }

    //used by reset game button in end game screen. starts game over
    public void DoRestartGame()
    {
        gameOver = false;
        EndGameScreen.SetActive(false);
        player1Time = 120;
        player2Time = 120;
        player1Score = 0;
        player2Score = 0;
        StartCoroutine("Timer");
        gameIsStopped = false;
        player1.timeIsUp = false;
        player2.timeIsUp = false;
        var activePowerUps = FindObjectsOfType<PowerUpBehaviour>();
        foreach (var powerUp in activePowerUps)
        {
            Destroy(powerUp.gameObject);
        }
        foreach (var customer in customers)
        {
            customer.ForceResetCustomer();
        }
    }

    //used by begin game button in start game screen. begins game
    public void DoStartGame()
    {
        gameStart = true;
        StartGameScreen.SetActive(false);
        StartCoroutine("Timer");
    }

    //waits a random amount dictated in RandomUtil to startcustomer. flag is to ensure this only happens once
    IEnumerator WaitToStartCustomer(CustomerBehaviour customer)
    {
        customer.isActive = true;
        yield return new WaitForSecondsRealtime(RandomUtil.GetRandomWaitTime());
        customer.StartCustomer();
    }

    //Tick seconds down if times are 0, stop game
    IEnumerator Timer()
    {
        secondPass = false;
        yield return new WaitForSecondsRealtime(1);
        Mathf.Clamp(--player1Time, 0, int.MaxValue);
        Mathf.Clamp(--player2Time, 0, int.MaxValue);
        if (player1Time < 0) player1Time = 0;
        if (player2Time < 0) player2Time = 0;
        if (player1Time == 0 && player2Time == 0)
        {
            gameOver = true;
        }
        secondPass = true;
    }
}
