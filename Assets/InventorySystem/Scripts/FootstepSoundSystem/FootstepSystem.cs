using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.iOS;

public class FootstepSystem : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //Footstep Audio System - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Footstep System -
    public TerrainTextureCheck terrainTextureChecker => GetComponent<TerrainTextureCheck>();
    public PlayerController playerObject => GetComponent<PlayerController>();

    [Header("Footstep Audio Database")]
    public List<FootStepAudioClipAsset> audioDatabase;
    #endregion

    #region - Footstep System Behavior -
    public float currentSpeed;
    public float modifier = 0.5f;
    private float distanceCovered;

    private AudioClip previusClip;

    private bool playerJumped = false;
    private float jumpAirTime = 0f;
    #endregion

    #region - Footstep Audio System Main Behavior -
    private void Update()//This method execut all the Footstep Audio System behavior every frame
    {
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
    #endregion

    #region - Item Clip Gethering -
    AudioClip GetClipFromArray(List<AudioClip> audioClips)//This method get an random clip from a list avoiding the anterior selected clip.
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
    #endregion

    #region - Footstep Audio System Behavior -

    #region - Sound Clip Selection by Texture -
    void TriggerNextClip()//This method trigger select the audioclip from Footstep AudioClip DataBase
    {
        if (playerObject.isOnTerrain)
        {
            terrainTextureChecker.GetTerrainTexture();
            //The above
            //is a nest of instructions that detect the current steppet texture getting it in the TerrainTextureChecker, later use the GetClipFromArray method that recieve the audiobase ReturnFullClipListByType method return using as indentifier the texture name/tag
            
            if (playerObject.isWalking && !playerObject.isRunning)//This statement plays the walk clips
            {               
                foreach(var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Walk")), selectedTexture.TextureValue);
            }
            else if (playerObject.isRunning)//This statement plays the run clips
            {
                foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("Run")), selectedTexture.TextureValue);
            }
        }
    }
    #endregion

    #region - Jump Sound Clip Selection by Texture -
    private void JumpBehaviorSoundDesing()//This method execut the jump footstep sound behavior 
    {
        if (playerObject.isOnTerrain)
        {
            if (playerObject.isGrounded && Input.GetKeyDown(GameManager.Instance.GeneralKeyCodes.GetKeyCodeByName("JumpKey")) && !playerJumped)//This statement represent the jump start sound behavior
            {
                foreach (var selectedTexture in terrainTextureChecker.textureValues) if (selectedTexture.TextureValue > 0) AudioManager.Instance.PlayFootstepSound(GetClipFromArray(audioDatabase.First(audioBase => audioBase.ListTag.Equals(selectedTexture.textureName)).ReturnFullClipListByType("JumpStart")), selectedTexture.TextureValue);
                playerJumped = true;
            }
            else if (!playerObject.isGrounded && playerJumped) jumpAirTime += Time.deltaTime;

            if (jumpAirTime > 0.15f)//This statement represent the jump land sound behavior 
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
    #endregion

    #endregion
}