using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;

    [Header("Player Attribues")]
    public float speed = 10.0F;
    public int playerNumber = 0;

    public Queue<Vegetable> heldItems = new Queue<Vegetable>(2);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = 0F, deltaY = 0F;
        #region Movement
        if (playerNumber == 1)
        {
            if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Left]))
            {
                deltaX = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Right]))
            {
                deltaX = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Left]) &&
                Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Right]))
            {
                deltaX = 0;
            }
            if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Up]))
            {
                deltaY = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Down]))
            {
                deltaY = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Up]) &&
                Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Down]))
            {
                deltaY = 0;
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Left]))
            {
                deltaX = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Right]))
            {
                deltaX = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Left]) &&
                Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Right]))
            {
                deltaX = 0;
            }
            if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Up]))
            {
                deltaY = 1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Down]))
            {
                deltaY = -1 * speed * Time.deltaTime;
            }
            else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Up]) &&
                Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Down]))
            {
                deltaY = 0;
            }

        }
        controller.Move(new Vector3(deltaX, deltaY));
        #endregion
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals(GameConstants.TrashTag))
        {
            Debug.Log(string.Format("{0} is near a trash can", name));
        }
    }
}
