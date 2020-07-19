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

    private float aspect = 0.0F;

    private bool secondPass = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        player1Time = 120;
        player2Time = 120;
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
        dispenserPositions.Add(new Vector3(minX + 0.5F, 0, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, 0, -2));
        dispenserPositions.Add(new Vector3(minX + 0.5F, minY * 1 / 3F, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, minY * 1 / 3F, -2));
        dispenserPositions.Add(new Vector3(minX + 0.5F, minY * 2 / 3F, -2));
        dispenserPositions.Add(new Vector3(maxX - 0.5F, minY * 2 / 3F, -2));
        var vegetables = new List<Vegetable>(GameConstants.Vegetables);
        RandomUtil.Shuffle(vegetables);
        for (int i = 0; i < vegetables.Count; ++i)
        {
            GameObject dispenser = Instantiate(DispensingStation);
            dispenser.transform.position = dispenserPositions[i];
            dispenser.GetComponent<VegetableDispenserBehaviour>().vegetableType = vegetables[i];
        }
        StartCoroutine("Timer");
    }

    // Update is called once per frame0
    void Update()
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder1.Append("Time: " + player1Time);
        stringBuilder1.Append("\nScore: " + player1Score);
        stringBuilder2.Append("Time: " + player2Time);
        stringBuilder2.Append("\nScore: " + player2Score);
        Player1Text.GetComponent<TextMesh>().text = stringBuilder1.ToString();
        Player2Text.GetComponent<TextMesh>().text = stringBuilder2.ToString();
        if (secondPass && !gameOver) StartCoroutine("Timer");
        if (player1Time == 0 && player2Time == 0)
        {
            DoStopGame();
        }
        if (!gameOver)
        {
            customers.ForEach(customer =>
            {
                if (!customer.isActive) StartCoroutine("WaitToStartCustomer", customer);
            });
        }
        if (Camera.main.aspect != aspect)
        {
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
        }
    }

    void DoStopGame()
    {
        Debug.Log("Game Over!");
        StopAllCoroutines();
        foreach (var customer in customers)
        {
            customer.isActive = false;

        }
        //Do Stop Game
    }

    IEnumerator WaitToStartCustomer(CustomerBehaviour customer)
    {
        customer.isActive = true;
        yield return new WaitForSecondsRealtime(RandomUtil.GetRandomWaitTime());
        customer.StartCustomer();
    }

    IEnumerator Timer()
    {
        secondPass = false;
        yield return new WaitForSecondsRealtime(1);
        Mathf.Clamp(--player1Time, 0, int.MaxValue);
        Mathf.Clamp(--player2Time, 0, int.MaxValue);
        if (player1Time == 0 && player2Time == 0)
        {
            gameOver = true;
        }
        secondPass = true;
    }
}
