using UnityEngine;
public class MainSceneController : MonoBehaviour
{
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
}
