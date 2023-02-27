using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumStorage : MonoBehaviour
{  }

public enum PlayerStateType 
{ 
    Stand, 
    Crouch, 
    Prone 
}
public enum GunType
{
    Shotgun,   
    SemiAndAuto,
    OnlySemi,
    SniperType
}
public enum GunState
{
    Locked,
    AutoFire,
    SemiFire,
    BurstFire
}

[Serializable]
public struct PlayerState
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