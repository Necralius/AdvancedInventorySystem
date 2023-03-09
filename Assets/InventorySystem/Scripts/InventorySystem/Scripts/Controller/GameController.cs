using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //Game Controller - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static GameController Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -
    public Transform playerBody;

    List<ItemEquip> equipList = new List<ItemEquip> { ItemEquip.NONE, ItemEquip.ARMOR_KEVLAR, ItemEquip.ARMOR_BALISTIC_HELMET,
            ItemEquip.Handable_FireGun_AK47, ItemEquip.Handable_FireGun_M4A4_Carbine, ItemEquip.Handable_FireGun_M9,ItemEquip.Handable_FireGun_M24,
            ItemEquip.Handable_FireGun_Glock17,ItemEquip.Handable_FireGun_UMP45 };//This variable represent all types of gun equip

    #endregion

    //=========== Method Area ===========//

    #region - Popup Show -
    //The above method literally execute the popup action, activating all the UI and passing all the instructions that the action need
    public void ShowPopup(string message, Sprite icon, Color textColor, Color backgroundColor, float timeToClosePopup) => PopUpView.Instance.PopupUpdate(message, icon, textColor, backgroundColor, timeToClosePopup);
    #endregion

    #region - Character Handables Equipping -
    public void EquipChar(ItemEquip itemEquip)//This method equip the weapon using the armument as the instruction to what weapon gonna be equiped
    {
        List<ItemEquip> equipList = new List<ItemEquip> { ItemEquip.NONE, ItemEquip.ARMOR_KEVLAR, ItemEquip.ARMOR_BALISTIC_HELMET,
            ItemEquip.Handable_FireGun_AK47, ItemEquip.Handable_FireGun_M4A4_Carbine, ItemEquip.Handable_FireGun_M9,ItemEquip.Handable_FireGun_M24,
            ItemEquip.Handable_FireGun_Glock17,ItemEquip.Handable_FireGun_UMP45 };

        foreach (ItemEquip equipType in equipList)
        {
            if (equipType.Equals(itemEquip)) SetEquipment(equipType, true);
            else SetEquipment(equipType, false);
        }
    }
    public void DequipAllWeapons()//This method dequip all the other weapons, is used as an guarantee that never gonna exist two or more weapons equipped at the same time
    {
        foreach(ItemEquip equipType in equipList) SetEquipment(equipType, false);
        PlayerController.Instance.equippedGun = null;
    }
    private void SetEquipment(ItemEquip itemEquip, bool activeState) => PlayerView.Instance.Equip(itemEquip, activeState);//This method literally activate the weapon passed by the argument itemEquip
    #endregion

    #region - Play Audio -
    public void PlayAudio(AudioClip audioClip) => AudioView.Instance.Play(audioClip);//This method plays an audio using the audio view class
    #endregion

    #region - Player Aspects Managment -
    public void CurePlayer(float value) => SurvivalAtributes.Instance.CurePlayer(value);//This method tries to cure the player using the SurvivalAtributes class
    public void DamagePlayer(float value) => SurvivalAtributes.Instance.GiveDamage(value);//This method tires to damage the player using the SruvivalAtributes class
    public void EatFood(float hungryValue, float thirstValue, float staminaValue)//This method change the hungry, thirst and stamina values using the SurvivalAtributes class
    {
        SurvivalAtributes playerAspectsAsset = SurvivalAtributes.Instance;

        if (playerAspectsAsset.currentHungry + hungryValue < 0) playerAspectsAsset.currentHungry = 0;
        else if (playerAspectsAsset.currentHungry + hungryValue > playerAspectsAsset.maxHungry) playerAspectsAsset.currentHungry = playerAspectsAsset.maxHungry;

        if (playerAspectsAsset.currentThrist + thirstValue < 0) playerAspectsAsset.currentThrist = 0;
        else if (playerAspectsAsset.currentThrist + thirstValue > playerAspectsAsset.maxThrist) playerAspectsAsset.currentThrist = playerAspectsAsset.maxThrist;

        if (playerAspectsAsset.currentStamina + staminaValue < 0) playerAspectsAsset.currentStamina = 0;
        else if (playerAspectsAsset.currentStamina + staminaValue > playerAspectsAsset.maxStamina) playerAspectsAsset.currentStamina = playerAspectsAsset.maxStamina;
    }
    public void TeleportPlayer(Transform newPos)//This method teleport the player to an new position and rotation
    {
        playerBody.position = newPos.position;
        playerBody.rotation = newPos.rotation;
    }
    #endregion

}