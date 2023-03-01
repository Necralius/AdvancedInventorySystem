using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static AudioView Instance;
    private void Awake() => Instance = this;
    #endregion

    public void Play(AudioClip audioClip) => AudioManager.Instance.PlayEffectSound(audioClip);

}