using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

class PlayerConstants
{
    public enum PlayerControls
    {
        Player1Left,
        Player1Right,
        Player1Up,
        Player1Down,
        Player1Interact,

        Player2Left,
        Player2Right,
        Player2Up,
        Player2Down,
        Player2Interact,


    }

    public static readonly Dictionary<PlayerControls, KeyCode> ControlsDict = new Dictionary<PlayerControls, KeyCode>
    {
        { PlayerControls.Player1Left, KeyCode.A },
        { PlayerControls.Player1Right, KeyCode.D },
        { PlayerControls.Player1Up, KeyCode.W },
        { PlayerControls.Player1Down, KeyCode.S },
        { PlayerControls.Player1Interact, KeyCode.LeftShift },

        { PlayerControls.Player2Left, KeyCode.LeftArrow },
        { PlayerControls.Player2Right, KeyCode.RightArrow },
        { PlayerControls.Player2Up, KeyCode.UpArrow },
        { PlayerControls.Player2Down, KeyCode.DownArrow },
        { PlayerControls.Player2Interact, KeyCode.RightControl },
    };


}
