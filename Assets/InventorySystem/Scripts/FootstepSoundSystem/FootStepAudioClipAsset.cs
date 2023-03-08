using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Footstep AudioClip Database", menuName = "FpsProject/AudioSystem/New Footstep AudioClip Database")]
public class FootStepAudioClipAsset : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //FootstepAudioClipAsset - Code Update Version 0.1 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.


    //This code represent an Footstep audioclip scriptable object, this asset store all the audio data and his indentification.

    public string ListTag;
    public List<AudioClip> WalkClips;
    public List<AudioClip> RunClips;
    public List<AudioClip> JumpStartClips;
    public List<AudioClip> JumpLandClips;

    public List<AudioClip> ReturnFullClipListByType(string Type)//This method returns the selected audio list based on his type, an string argument select the certain list to return. 
    {
        if (Type == "Walk") return WalkClips;
        else if (Type == "Run") return RunClips;
        else if (Type == "JumpStart") return JumpStartClips;
        else if (Type == "JumpLand") return JumpLandClips;
        return null;
    }
    public AudioClip GetRandomClipFromList(string Type) => ReturnFullClipListByType(Type)[Random.Range(0, ReturnFullClipListByType(Type).Count)];//This method use the ReturnFullClipListByType method to take an random audio clip from the list an return it
}