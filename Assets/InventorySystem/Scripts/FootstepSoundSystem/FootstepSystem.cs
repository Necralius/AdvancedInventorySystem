using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootstepSystem : MonoBehaviour
{
    public TerrainTextureCheck terrainTextureChecker => GetComponent<TerrainTextureCheck>();
    public List<FootStepAudioClipAsset> audioDatabase;

    public PlayerController playerObject => GetComponent<PlayerController>();
    
    public float currentSpeed;
    float distanceCovered;
    public float modifier = 0.5f;

    private AudioClip previusClip;

    float airTime;

    private void Update()
    {
        PlaySoundIfFalling();
        if (playerObject.isWalking)
        {
            distanceCovered += (currentSpeed * Time.deltaTime) * modifier;
            if (distanceCovered > 1)
            {
                TriggerNextClip();
                distanceCovered = 0;
            }
        }
    }

    AudioClip GetClipFromArray(List<AudioClip> audioClips)
    {
        int attempts = 3;
        AudioClip selectedClip = audioClips[Random.Range(0, audioClips.Count - 1)];

        while(selectedClip == previusClip && attempts > 0)
        {
            selectedClip = audioClips[Random.Range(0, audioClips.Count - 1)];
            attempts--;
        }
        previusClip = selectedClip;
        return selectedClip;
    }
    void TriggerNextClip()
    {
        terrainTextureChecker.GetTerrainTexture();
        if (playerObject.isOnTerrain && playerObject.isWalking && !playerObject.isRunning)
        {
            Debug.Log("Returning walk Clips!");
            foreach(var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Walk")), selectedTexture.TextureValue);
        }
        else if (playerObject.isOnTerrain && playerObject.isRunning)
        {
            Debug.Log("Returning Run Clips!");
            foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Run")), selectedTexture.TextureValue);
        }
    }
    private void PlaySoundIfFalling()
    {
        if (!playerObject.isGrounded) airTime += Time.deltaTime;
        else
        {
            if (airTime > 0.25f)
            {
                TriggerNextClip();
                airTime = 0;
            }
        }
    }
}