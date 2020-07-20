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

    public bool timeIsUp = false;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        text = GetComponentInChildren<TextMesh>();
    }

    void Update()
    {
        didInteractThisFrame = false;
        float deltaX = 0F, deltaY = 0F;
        if (!isChopping && !timeIsUp)
        {
            #region Movement
            //Handle movement for each player according to the controls spelled out in PlayerConstants.ControlsDict
            if (playerNumber == 1)
            {
                //if left & right, do not move
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
                //if up & down, do not move
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
                //if left & right, do not move
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
                //if up & down, do not move
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
            //Handle interaction for each player
            //didInteractThisFrame is a sentinel used to stop repeat actions based on frame border handling
            //other flags are if the player is near an interactible object
            //Handle drop interaction
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
            //Handle pickup interaction
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

    //Handle trigger collisions, which tell the player what it is near
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

    //Handle trigger leave collisions, which let the player know they are not near something now
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            UpdateNear(other.tag, false);
            if (!isNearTrash && !isNearDispenser && !isNearCuttingBoard && !isNearCustomer && !isNearPlate) nearObject = null;
        }
    }

    //Set the near object to the most recent near object
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

    //Handle the actual interaction
    void TryInteract(PlayerConstants.InteractMode mode, GameObject nearObj)
    {
        //Modes are drop and pickup
        switch (mode)
        {
            case PlayerConstants.InteractMode.Drop:
                //the player has something to drop?
                if (heldObjects.Count > 0)
                {
                    switch (nearObj.tag)
                    {
                        //player is dropping object onto plate
                        case GameConstants.PlateTag:
                            PlateBehaviour plate = nearObj.GetComponent<PlateBehaviour>();
                            //plate has no object stored
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
                        //player is dropping object onto customer
                        case GameConstants.CustomerTag:
                            CustomerBehaviour customer = nearObj.GetComponent<CustomerBehaviour>();
                            //customer is active and ready for food
                            if (customer.canAcceptFood)
                            {
                                //dequeue object, getting ready to put it in front of customer
                                var activeObject = heldObjects.Dequeue();
                                if (activeObject is Salad salad1)
                                {
                                    if (customer.submittedFood == null)
                                    {
                                        customer.submittedFood = salad1;
                                        customer.submittingPlayer = this;
                                    }
                                    //this shouldn't happen, but if there is a salad there already, put the salad we attempt to put there back
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
                                //we can't give customers raw ingredients, put the thing back if it is not a salad
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
                            didInteractThisFrame = true;
                            break;
                        //player is dropping object onto cutting board
                        case GameConstants.CuttingBoardTag:
                            CuttingBoardBehaviour cuttingBoard = nearObj.GetComponent<CuttingBoardBehaviour>();
                            //the cutting board is not busy, so objects can be dropped on it
                            if (!cuttingBoard.working)
                            {
                                var activeObject = heldObjects.Dequeue();
                                if (activeObject is Salad salad1)
                                {
                                    if (cuttingBoard.activeSalad == null)
                                    {
                                        cuttingBoard.activeSalad = new Salad(salad1);
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
                                    //cutting board has no ingredients, and is ready to start chopping. Do so.
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
                        //player is dropping object onto trash
                        case GameConstants.TrashTag:
                        default:
                            //get scene controller
                            var mainController = FindObjectOfType<MainSceneController>();
                            var droppedObject = heldObjects.Dequeue();
                            //deduct points based on what player is throwing away
                            if (droppedObject is Salad salad)
                            {
                                switch (playerNumber)
                                {
                                    case 1:
                                        mainController.player1Score -= salad.GetIngredientCount() * GameConstants.ScorePerIngredient;
                                        break;
                                    case 2:
                                        mainController.player2Score -= salad.GetIngredientCount() * GameConstants.ScorePerIngredient;
                                        break;
                                }
                            }
                            else if (droppedObject is Vegetable)
                            {
                                switch (playerNumber)
                                {
                                    case 1:
                                        mainController.player1Score -= GameConstants.ScorePerIngredient;
                                        break;
                                    case 2:
                                        mainController.player2Score -= GameConstants.ScorePerIngredient;
                                        break;
                                }
                            }
                            break;
                    }
                }
                break;
            //picking up food
            case PlayerConstants.InteractMode.PickUp:
                switch (nearObj.tag)
                {
                    //player is picking up from vegetable dispenser
                    case GameConstants.DispenserTag:
                        VegetableDispenserBehaviour vegetableDispenser = nearObj.GetComponent<VegetableDispenserBehaviour>();
                        //player has room for another item?
                        if (heldObjects.Count < 2)
                        {
                            heldObjects.Enqueue(new Vegetable(vegetableDispenser.vegetableType));
                        }
                        didInteractThisFrame = true;
                        break;
                    //player is picking up from plate
                    case GameConstants.PlateTag:
                        PlateBehaviour plate = nearObj.GetComponent<PlateBehaviour>();
                        //plate has item and player can hold it?
                        if (plate.heldObject.name != "" && heldObjects.Count < 2)
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
                        //cutting board is not busy?
                        if (!cuttingBoard.working)
                        {
                            //cutting board has item to pick up and player can hold it?
                            if (cuttingBoard.activeSalad != null && heldObjects.Count < 2)
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
        //set player text to the names of the items held
        text.text = stringBuilder.ToString();
    }

    //do chopping coroutine, used to freeze player for duration of GameConstants.ChopTime
    IEnumerator DoChopping(CuttingBoardBehaviour cuttingBoard)
    {
        yield return new WaitForSecondsRealtime(GameConstants.ChopTime);
        cuttingBoard.CreateSaladFromIngredient();
        isChopping = false;
        cuttingBoard.working = false;
    }
}
