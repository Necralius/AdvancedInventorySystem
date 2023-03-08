using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //Audio Manager - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static AudioManager Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Audio Sources -
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    #endregion

    #region - Sound Play Methods -

    #region - Default Sound Play Methods -
    public void PlayEffectSound(AudioClip clipToPlay) => EffectsSource.PlayOneShot(clipToPlay);//This method play an AudioClip using the effect AudioSource
    public void PlayMusicSound(AudioClip clipToPlay) => MusicSource.PlayOneShot(clipToPlay);//This method play an AudioClip using the music AudioSource
    #endregion

    #region - Shoot Sound System -
    public void PlayShootSound(AudioClip clipToPlay, float minPitch, float maxPitch, float minVolume, float maxVolume)//This method is an overload of the default PlayShootSound method that permit the system user to set an range of the pitch and volume randomizer
    {
        EffectsSource.pitch = Random.Range(minPitch, maxPitch);
        EffectsSource.volume = Random.Range(minVolume, maxVolume);
        EffectsSource.PlayOneShot(clipToPlay);
    }
    public void PlayShootSound(AudioClip clipToPlay)//This method play an shoot sound considering an random variation in pitch and volume
    {
        EffectsSource.pitch = Random.Range(0.8f, 1f);
        EffectsSource.volume = Random.Range(0.85f, 1f);
        EffectsSource.PlayOneShot(clipToPlay);
    }
    #endregion

    #region - Footste Sound System -
    public void PlayFootstepSound(AudioClip clipToPlay, float volumeScale)//This method play the footstep sound, using a variation in the pitch and volume
    {
        EffectsSource.pitch = Random.Range(0.8f, 1f);
        EffectsSource.volume = Random.Range(0.85f, 1f);
        EffectsSource.PlayOneShot(clipToPlay, volumeScale);
    }
    #endregion

    #endregion
}