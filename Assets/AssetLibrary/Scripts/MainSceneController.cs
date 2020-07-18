using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    public List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    [SerializeField]
    public List<Vector3> dispenserPositions = new List<Vector3>();

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

    private float aspect = 0.0F;

    // Start is called before the first frame update
    void Start()
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
        TopCounter.transform.position = new Vector3(0, maxY - 1.5F, -1.25F);
        BottomCounter.transform.position = new Vector3(0, minY + 0.5F, -1.5F);
        RightCounter.transform.position = new Vector3(maxX - 0.5F, 0, -1.5F);
        LeftCounter.transform.position = new Vector3(minX + 0.5F, 0, -1.5F);

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
    }

    // Update is called once per frame0
    void Update()
    {
        customers.ForEach(customer =>
        {
            if (!customer.isActive) StartCoroutine("WaitToStartCustomer", customer);
        });
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

    IEnumerator WaitToStartCustomer(CustomerBehaviour customer)
    {
        customer.isActive = true;
        yield return new WaitForSecondsRealtime(RandomUtil.GetRandomWaitTime());
        customer.StartCustomer();
    }
}
