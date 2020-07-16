using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController character;
    public float speed = 3.0F;
    public int playerNumber = 0;
    private Vector3 movement = new Vector3(0, 0, 0);
    private Vector2 moveTo = new Vector2(0, 0);


    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = 0F, deltaY = 0F;
        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.A))
            {
                deltaX = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                deltaX = 1 * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                deltaY = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                deltaY = -1 * speed * Time.deltaTime;
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                deltaX = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                deltaX = 1 * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                deltaY = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                deltaY = -1 * speed * Time.deltaTime;
            }
        }

        moveTo = new Vector2(deltaX, deltaY);
        movement = new Vector3(moveTo.x, moveTo.y, 0);
        character.Move(movement);
    }
}
