using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    private Transform position;
    public int playerNumber = 0;
    public Queue<Vegetable> heldItems = new Queue<Vegetable>(2);
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
    }

    //Grabbing should only be done when an object to grab is near, and dropping should only be done when a place to drop an item is near
    //I.E. cannot grab air, cannot drop onto floor
    void Update()
    {
        if (playerNumber == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TryInteract(playerNumber);
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                TryInteract(playerNumber);
            }
        }
    }

    void TryInteract(int playerNumber)
    {
        //player is near pick up point

        //player is near trash or drop off
        Debug.Log(string.Format("Player {0} interacted with the game :D", playerNumber));
        Debug.Log(string.Format("Player {0} is at position {1}, {2}, {3}", playerNumber, position.position.x, position.position.y, position.position.z));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
