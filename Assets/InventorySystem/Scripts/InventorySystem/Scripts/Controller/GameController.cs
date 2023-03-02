using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region - Singleton Pattern -
    public static GameController Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -
    public Transform playerBody;

    #endregion

    #region - Methods -

    public void ShowPopup(string message, Sprite icon, Color textColor, Color backgroundColor, float timeToClosePopup)
    {
        PopUpView.Instance.PopupUpdate(message, icon, textColor, backgroundColor, timeToClosePopup);
    }
    public void EquipChar(ItemEquip itemEquip)
    {
        switch (itemEquip)
        {
            case ItemEquip.NONE:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.ARMOR_KEVLAR:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.ARMOR_BALISTIC_HELMET:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_AK47:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_M4A4_Carbine:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_M9:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_M24:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_Glock17:
                SetEquipment(itemEquip);
                break;
            case ItemEquip.Handable_FireGun_UMP45:
                SetEquipment(itemEquip);
                break;
            default:
                break;
        }
    }
    public void PlayAudio(AudioClip audioClip) => AudioView.Instance.Play(audioClip);
    private void SetEquipment(ItemEquip itemEquip)
    {
        bool status = PlayerView.Instance.StatusEquipment(itemEquip);
        PlayerView.Instance.Equip(itemEquip, !status);
    }

    public void CurePlayer(float value) => SurvivalAtributes.Instance.CurePlayer(value);
    public void DamagePlayer(float value) => SurvivalAtributes.Instance.GiveDamage(value);
    public void EatFood(float hungryValue, float thirstValue, float staminaValue)
    {
        SurvivalAtributes playerAspectsAsset = SurvivalAtributes.Instance;

        if (playerAspectsAsset.currentHungry + hungryValue < 0) playerAspectsAsset.currentHungry = 0;
        else if (playerAspectsAsset.currentHungry + hungryValue > playerAspectsAsset.maxHungry) playerAspectsAsset.currentHungry = playerAspectsAsset.maxHungry;

        if (playerAspectsAsset.currentThrist + thirstValue < 0) playerAspectsAsset.currentThrist = 0;
        else if (playerAspectsAsset.currentThrist + thirstValue > playerAspectsAsset.maxThrist) playerAspectsAsset.currentThrist = playerAspectsAsset.maxThrist;

        if (playerAspectsAsset.currentStamina + staminaValue < 0) playerAspectsAsset.currentStamina = 0;
        else if (playerAspectsAsset.currentStamina + staminaValue > playerAspectsAsset.maxStamina) playerAspectsAsset.currentStamina = playerAspectsAsset.maxStamina;
    }
    public void TeleportPlayer(Transform newPos)
    {
        playerBody.position = newPos.position;
        playerBody.rotation = newPos.rotation;
    }
    #endregion



}