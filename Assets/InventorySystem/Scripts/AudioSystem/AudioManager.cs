using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static AudioManager Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Audio Sources -
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    public AudioSource VoiceSource;
    #endregion

    #region - Sound Play Methods -
    public void PlayEffectSound(AudioClip clipToPlay) => EffectsSource.PlayOneShot(clipToPlay);
    public void PlayMusicSound(AudioClip clipToPlay) => MusicSource.PlayOneShot(clipToPlay);
    public void PlayVoiceSound(AudioClip clipToPlay) => VoiceSource.PlayOneShot(clipToPlay);
    public void PlayShootSound(AudioClip clipToPlay, float minPitch, float maxPitch, float minVolume, float maxVolume)
    {
        EffectsSource.pitch = Random.Range(minPitch, maxPitch);
        EffectsSource.volume = Random.Range(minVolume, maxVolume);
        EffectsSource.PlayOneShot(clipToPlay);
    }
    public void PlayShootSound(AudioClip clipToPlay)
    {
        EffectsSource.pitch = Random.Range(0.8f, 1f);
        EffectsSource.volume = Random.Range(0.85f, 1f);
        EffectsSource.PlayOneShot(clipToPlay);
    }
    #endregion
}