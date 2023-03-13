using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumStorage : MonoBehaviour
{  }
//Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
//EnumStorage - Code Update Version 0.5 - (Refactored code).
//Feel free to take all the code logic and apply in yours projects.
//This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

#region - Player State Type - 
public enum PlayerStateType //This enumerator represents all the possible player state types
{ 
    Stand,
    Crouch,
    Prone
}
#endregion

#region - Gun Type -
public enum GunType //This enumerator represents all the gun possible types
{
    Shotgun,   
    SemiAndAuto,
    OnlySemi,
    SniperType
}
public enum GunTypeIndexer
{
    Pistol = 0,
    Rifle = 1
}
#endregion

#region - Gun State -
public enum GunState //This enumerator represents all the gun possible States
{
    Locked,
    AutoFire,
    SemiFire,
    BurstFire
}
#endregion

#region - Player State Model -
[Serializable]
public struct PlayerState //This
{
    public float StateHeight;
    public float StateSpeedModifier;
    public PlayerStateType state;

    public void SetUp(float height, float speedModifier, PlayerStateType state)
    {
        this.StateHeight = height;
        this.StateSpeedModifier = speedModifier;
        this.state = state;
    }
    public void SetUp(PlayerState state)
    {
        this.StateHeight = state.StateHeight;
        this.StateSpeedModifier = state.StateSpeedModifier;
        this.state = state.state;
    }
}
#endregion