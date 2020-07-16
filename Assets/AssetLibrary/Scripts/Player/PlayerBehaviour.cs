using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;
    private TextMesh text;

    [Header("Player Attribues")]
    public float speed = 10.0F;
    public int playerNumber = 0;

    [Header("Nearness Attributes")]
    public bool isNearDispenser = false;
    public bool isNearTrash = false;
    public bool isNearCustomer = false;
    public bool isNearCuttingBoard = false;

    private GameObject nearObject;
    
    public Queue<string> heldVegetables = new Queue<string>(2);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        text = GetComponentInChildren<TextMesh>();
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
        #region Interaction
        if (isNearTrash || isNearCuttingBoard || isNearCustomer)
        {
            if (playerNumber == 1)
            {
                if (Input.GetKeyDown(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Interact]))
                {
                    TryInteract(PlayerConstants.InteractMode.Drop, nearObject);
                }
            }
            else if (playerNumber == 2)
            {
                if (Input.GetKeyDown(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Interact]))
                {
                    TryInteract(PlayerConstants.InteractMode.Drop, nearObject);
                }
            }
        }
        if (isNearDispenser)
        {
            if (playerNumber == 1)
            {
                if (Input.GetKeyDown(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Interact]))
                {
                    TryInteract(PlayerConstants.InteractMode.PickUp, nearObject);
                }
            }
            else if (playerNumber == 2)
            {
                if (Input.GetKeyDown(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Interact]))
                {
                    TryInteract(PlayerConstants.InteractMode.PickUp, nearObject);
                }
            }

        }
        #endregion

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            if (other.CompareTag(GameConstants.TrashTag))
            {
                isNearTrash = true;
                nearObject = other.gameObject;
            }
            if (other.CompareTag(GameConstants.DispenserTag))
            {
                isNearDispenser = true;
                nearObject = other.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            if (other.CompareTag(GameConstants.TrashTag)) isNearTrash = false;
            if (other.CompareTag(GameConstants.DispenserTag)) isNearDispenser = false;
            if (!isNearTrash && !isNearDispenser && !isNearCuttingBoard && !isNearCustomer) nearObject = null;
        }
    }

    void TryInteract(PlayerConstants.InteractMode mode, GameObject nearObj)
    {
        switch (mode)
        {
            case PlayerConstants.InteractMode.Drop:
                if (heldVegetables.Count > 0)
                {
                    string vegetable = heldVegetables.Dequeue();
                    Debug.Log(string.Format("{0} is dropping {1} into {2}", name, vegetable, nearObj.name));
                }
                break;
            case PlayerConstants.InteractMode.PickUp:
                VegetableDispenserBehaviour vegetableDispenser = nearObj.GetComponent<VegetableDispenserBehaviour>();
                if (heldVegetables.Count < 2)
                {
                    heldVegetables.Enqueue(vegetableDispenser.vegetableType);
                    Debug.Log(string.Format("{0} is picking up {1}", name, vegetableDispenser.vegetableType));
                }
                break;
        }
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string vegetable in heldVegetables) {
            stringBuilder.Append(string.Format("{0}\n", vegetable));
        }
        text.text = stringBuilder.ToString();
    }
}
