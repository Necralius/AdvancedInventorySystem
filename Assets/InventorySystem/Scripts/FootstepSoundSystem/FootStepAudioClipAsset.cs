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
    public string ListTag;
    public List<AudioClip> WalkClips;
    public List<AudioClip> RunClips;
    public List<AudioClip> JumpLandClips;

    public List<AudioClip> ReturnFullClipListByType(string Type)
    {
        Debug.Log("Returning List!");
        if (Type == "Walk") return WalkClips;
        else if (Type == "Run") return RunClips;
        else if (Type == "JumpLand") return JumpLandClips;
        return null;
    }
    public AudioClip GetRandomClipFromList(string Type) => ReturnFullClipListByType(Type)[Random.Range(0, ReturnFullClipListByType(Type).Count)];
}