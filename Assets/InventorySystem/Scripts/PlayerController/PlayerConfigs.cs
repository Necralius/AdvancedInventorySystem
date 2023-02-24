using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New KeyCodeList", menuName = "FpsProject/KeyCode/New Key Code")]
public class KeyCodeGroup : ScriptableObject
{
    public List<KeyCodeSave> keyCodes;
    public KeyCode GetKeyCodeByName(string name) => keyCodes.Find(x => x.keyActionName == name).actionKeyCode;
}

[Serializable]
public class PlayerStats
{
    [Range(1, 15)] public float mouseSensitivity;

    public float walkSpeed;
    public float runSpeed;

    public float currentSpeedEffector;
    public float standSpeedEffector;
    public float crouchSpeedEffector;
    public float proneSpeedEffector;
    public float aimSpeedEffector;
}

[Serializable]
public struct KeyCodeSave
{
    public string keyActionName;
    public KeyCode actionKeyCode;
}