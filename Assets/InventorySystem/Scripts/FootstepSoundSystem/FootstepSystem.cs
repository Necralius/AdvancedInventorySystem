using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.iOS;

public class FootstepSystem : MonoBehaviour
{
    #region - Footstep System -
    public TerrainTextureCheck terrainTextureChecker => GetComponent<TerrainTextureCheck>();
    public PlayerController playerObject => GetComponent<PlayerController>();
    [Header("Footstep Audio Database")]
    public List<FootStepAudioClipAsset> audioDatabase;
    #endregion

    public float currentSpeed;
    public float modifier = 0.5f;
    private float distanceCovered;

    private AudioClip previusClip;

    private bool playerJumped = false;
    private float jumpAirTime = 0f;

    float airTime;

    private void Update()
    {
        PlaySoundIfFalling();
        JumpBehaviorSoundDesing();
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
        if (playerObject.isOnTerrain)
        {
            terrainTextureChecker.GetTerrainTexture();
            if (playerObject.isWalking && !playerObject.isRunning)
            {
                foreach(var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Walk")), selectedTexture.TextureValue);
            }
            else if (playerObject.isRunning)
            {
                foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Run")), selectedTexture.TextureValue);
            }
        }
    }
    private void JumpBehaviorSoundDesing()
    {
        if (playerObject.isOnTerrain)
        {
            if (playerObject.isGrounded && Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("JumpKey")) && !playerJumped)
            {
                foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("JumpStart")), selectedTexture.TextureValue);
                playerJumped = true;
            }
            else if (!playerObject.isGrounded && playerJumped) jumpAirTime += Time.deltaTime;

            if (jumpAirTime > 0.15f)
            {
                if (playerObject.isGrounded)
                {
                    foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("JumpLand")), selectedTexture.TextureValue);
                    jumpAirTime = 0;
                    playerJumped = false;
                }
            }
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