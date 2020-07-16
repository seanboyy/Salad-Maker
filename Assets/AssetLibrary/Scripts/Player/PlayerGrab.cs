using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public int playerNumber = 0;
    public bool isHolding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Grabbing should only be done when an object to grab is near, and dropping should only be done when a place to drop an item is near
    //I.E. cannot grab air, cannot drop onto floor
    void Update()
    {
        if (playerNumber == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isHolding = !isHolding;
                if (isHolding) Debug.Log("Player One Grabbed Something!");
                else Debug.Log("Player One Dropped Something!");
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                isHolding = !isHolding;
                if (isHolding) Debug.Log("Player Two Grabbed Something!");
                else Debug.Log("Player Two Dropped Something!");
            }
        }
    }
}
