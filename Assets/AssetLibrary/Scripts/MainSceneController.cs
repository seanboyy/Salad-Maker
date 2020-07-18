using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    public List<CustomerBehaviour> customers = new List<CustomerBehaviour>();

    [Header("Prefabs")]
    public GameObject DispensingStation;
    [Header("Walls")]
    public GameObject TopWall;
    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject BottomWall;

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
    }

    // Update is called once per frame
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
