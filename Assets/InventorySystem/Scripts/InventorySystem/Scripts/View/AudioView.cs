using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //AudioView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static AudioView Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - View Behavior -
    public void Play(AudioClip audioClip) => AudioManager.Instance.PlayEffectSound(audioClip);//This method use the AudioManager to play an effect sound
    #endregion
}