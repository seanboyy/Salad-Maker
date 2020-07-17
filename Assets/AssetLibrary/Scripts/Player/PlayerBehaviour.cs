using System;
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
    public bool isNearPlate = false;

    [Header("Action Attributes")]
    public bool isChopping = false;

    public bool didInteractThisFrame = false;

    public GameObject nearObject;

    public Queue<ScoreObject> heldObjects = new Queue<ScoreObject>(2);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        text = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        didInteractThisFrame = false;
        float deltaX = 0F, deltaY = 0F;
        if (!isChopping)
        {
            #region Movement
            if (playerNumber == 1)
            {
                if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Left]) &&
                    Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Right]))
                {
                    deltaX = 0;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Left]))
                {
                    deltaX = -1 * speed * Time.deltaTime;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Right]))
                {
                    deltaX = 1 * speed * Time.deltaTime;
                }
                if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Up]) &&
                    Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Down]))
                {
                    deltaY = 0;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Up]))
                {
                    deltaY = 1 * speed * Time.deltaTime;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player1Down]))
                {
                    deltaY = -1 * speed * Time.deltaTime;
                }
            }
            else if (playerNumber == 2)
            {
                if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Left]) &&
                    Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Right]))
                {
                    deltaX = 0;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Left]))
                {
                    deltaX = -1 * speed * Time.deltaTime;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Right]))
                {
                    deltaX = 1 * speed * Time.deltaTime;
                }
                if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Up]) &&
                    Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Down]))
                {
                    deltaY = 0;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Up]))
                {
                    deltaY = 1 * speed * Time.deltaTime;
                }
                else if (Input.GetKey(PlayerConstants.ControlsDict[PlayerConstants.PlayerControls.Player2Down]))
                {
                    deltaY = -1 * speed * Time.deltaTime;
                }

            }
            controller.Move(new Vector3(deltaX, deltaY));
            #endregion
            #region Interaction
            if (!didInteractThisFrame && (isNearTrash || (isNearCuttingBoard && heldObjects.Count > 0) || isNearCustomer || isNearPlate))
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
            if (!didInteractThisFrame && (isNearDispenser || (isNearCuttingBoard && heldObjects.Count == 0) || (isNearPlate && heldObjects.Count < 2)))
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            UpdateNear(other.tag, true);
            if (!other.CompareTag("Player"))
            {
                if (isNearCuttingBoard && isNearTrash)
                {
                    //Prioritise cutting board over trash can
                    if (other.CompareTag(GameConstants.CuttingBoardTag)) nearObject = other.gameObject;
                }
                else nearObject = other.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            UpdateNear(other.tag, false);
            if (!isNearTrash && !isNearDispenser && !isNearCuttingBoard && !isNearCustomer && !isNearPlate) nearObject = null;
        }
    }

    void UpdateNear(string otherTag, bool flag)
    {
        switch (otherTag)
        {
            case GameConstants.TrashTag:
                isNearTrash = flag;
                break;
            case GameConstants.DispenserTag:
                isNearDispenser = flag;
                break;
            case GameConstants.CustomerTag:
                isNearCustomer = flag;
                break;
            case GameConstants.CuttingBoardTag:
                isNearCuttingBoard = flag;
                break;
            case GameConstants.PlateTag:
                isNearPlate = flag;
                break;
        }
    }

    void TryInteract(PlayerConstants.InteractMode mode, GameObject nearObj)
    {
        switch (mode)
        {
            case PlayerConstants.InteractMode.Drop:
                if (heldObjects.Count > 0)
                {
                    switch (nearObj.tag)
                    {
                        case GameConstants.PlateTag:
                            PlateBehaviour plate = nearObj.GetComponent<PlateBehaviour>();
                            if (plate.heldObject.name == "")
                            {
                                var scoreObject = heldObjects.Dequeue();
                                if (scoreObject is Vegetable vegetable)
                                {
                                    plate.heldObject = new Vegetable(vegetable);
                                }
                                else
                                {
                                    plate.heldObject = new Salad((Salad)scoreObject);

                                }
                                didInteractThisFrame = true;
                            }
                            break;
                        case GameConstants.CustomerTag:
                            didInteractThisFrame = true;
                            break;
                        case GameConstants.CuttingBoardTag:
                            CuttingBoardBehaviour cuttingBoard = nearObj.GetComponent<CuttingBoardBehaviour>();
                            if (!cuttingBoard.working)
                            {
                                var activeObject = heldObjects.Dequeue();
                                if (activeObject is Salad salad)
                                {
                                    if (cuttingBoard.activeSalad == null)
                                    {
                                        cuttingBoard.activeSalad = new Salad(salad);
                                        didInteractThisFrame = true;
                                    }
                                    //there is already a salad there, so we shouldn't place one. Restore queue to previous state
                                    else
                                    {
                                        ScoreObject tempObject = null;
                                        if (heldObjects.Count > 0)
                                        {
                                            tempObject = heldObjects.Dequeue();
                                        }
                                        heldObjects.Enqueue(activeObject);
                                        if (tempObject != null) heldObjects.Enqueue(tempObject);
                                    }
                                }
                                else if (activeObject is Vegetable vegetable)
                                {
                                    if (cuttingBoard.ingredient.name == "")
                                    {
                                        cuttingBoard.ingredient = new Vegetable(vegetable);
                                        ProgressBarBehaviour progressBar = nearObj.GetComponentInChildren<ProgressBarBehaviour>();
                                        progressBar.StartProgressBar(GameConstants.ChopTime);
                                        isChopping = true;
                                        cuttingBoard.working = true;
                                        StartCoroutine("DoChopping", cuttingBoard);
                                        didInteractThisFrame = true;
                                    }
                                    //this shouldn't happen, but just in case restore queue to previous state
                                    else
                                    {
                                        ScoreObject tempObject = null;
                                        if (heldObjects.Count > 0)
                                        {
                                            tempObject = heldObjects.Dequeue();
                                        }
                                        heldObjects.Enqueue(activeObject);
                                        if (tempObject != null) heldObjects.Enqueue(tempObject);
                                    }
                                }
                            }
                            break;
                        case GameConstants.TrashTag:
                        default:
                            heldObjects.Dequeue();
                            break;
                    }
                }
                break;
            case PlayerConstants.InteractMode.PickUp:
                switch (nearObj.tag)
                {
                    case GameConstants.DispenserTag:
                        VegetableDispenserBehaviour vegetableDispenser = nearObj.GetComponent<VegetableDispenserBehaviour>();
                        if (heldObjects.Count < 2)
                        {
                            heldObjects.Enqueue(new Vegetable(vegetableDispenser.vegetableType));
                        }
                        didInteractThisFrame = true;
                        break;
                    case GameConstants.PlateTag:
                        PlateBehaviour plate = nearObj.GetComponent<PlateBehaviour>();
                        if (plate.heldObject.name != "")
                        {
                            if (plate.heldObject is Vegetable vegetable) {
                                heldObjects.Enqueue(new Vegetable(vegetable));
                            }
                            else if (plate.heldObject is Salad salad) {
                                heldObjects.Enqueue(new Salad(salad));
                            }
                            plate.heldObject = new ScoreObject();
                        }
                        didInteractThisFrame = true;
                        break;
                    case GameConstants.CuttingBoardTag:
                        CuttingBoardBehaviour cuttingBoard = nearObj.GetComponent<CuttingBoardBehaviour>();
                        if (!cuttingBoard.working)
                        {
                            if (cuttingBoard.activeSalad != null)
                            {
                                heldObjects.Enqueue(cuttingBoard.activeSalad);
                                cuttingBoard.activeSalad = null;
                                didInteractThisFrame = true;
                            }
                        }
                        break;
                }
                break;
        }
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var vegetable in heldObjects)
        {
            stringBuilder.Append(string.Format("{0}\n", vegetable.name));
        }
        text.text = stringBuilder.ToString();
    }

    IEnumerator DoChopping(CuttingBoardBehaviour cuttingBoard)
    {
        yield return new WaitForSecondsRealtime(GameConstants.ChopTime);
        cuttingBoard.CreateSaladFromIngredient();
        isChopping = false;
        cuttingBoard.working = false;
    }
}
