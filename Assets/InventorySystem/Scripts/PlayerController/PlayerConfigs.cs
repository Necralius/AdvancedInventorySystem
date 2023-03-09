using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region - KeyCode Group Asset Model -
[CreateAssetMenu(fileName = "New KeyCodeList", menuName = "FpsProject/KeyCode/New Key Code")]
public class KeyCodeGroup : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //KeyCodeGroup - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    public List<KeyCodeSave> keyCodes;//This variable represent an list of all keycodes used in the game functionalities
    public KeyCode GetKeyCodeByName(string name) => keyCodes.Find(x => x.keyActionName == name).actionKeyCode;//This method return the KeyCode value based on his tag
}
#endregion

#region - KeyCode Storage -
[Serializable]
public struct KeyCodeSave//This statements represnt the KeyCode data structure
{
    public string keyActionName;
    public KeyCode actionKeyCode;
}
#endregion

#region - Player Settings Storage -
[Serializable]
public class PlayerStats //This model represents the player settings storage
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //PlayerStats - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.
    [Range(1, 15)] public float mouseSensitivity;

    public float walkSpeed;
    public float runSpeed;

    public float currentSpeedEffector;
    public float standSpeedEffector;
    public float crouchSpeedEffector;
    public float proneSpeedEffector;
    public float aimSpeedEffector;
}
#endregion