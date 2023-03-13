using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //PlayerView - Code Update Version 0.4 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    public static PlayerView Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -
    [SerializeField] private List<GameObject> weaponGoList;

    [SerializeField] Slider characterRotationSlider;
    [SerializeField] Transform characterObject;
    [SerializeField] private float itemRotationSpeed;
    #endregion

    #region - Weapon Equip -
    public void Equip(ItemEquip typeWeapon, bool visible)//This method select one weapon and set his state to the argument passed, and update all the weapons UI an assing the weapon to the equippedGun on PlayerController
    {
        foreach(var item in weaponGoList)
        {
            if (item.name == typeWeapon.ToString())
            {
                if (visible)
                {
                    PlayerController.Instance.equippedGun = item.GetComponent<WeaponSystem>();
                    PlayerController.Instance.equippedGun.UpdateGunUI();
                }
                item.GetComponent<WeaponSystem>().SetGunState(visible);
            }
        }
    }
    #endregion

    #region - Status Gathering -
    public bool StatusEquipment(ItemEquip itemEquip)//This method select one weapon based on the argument, and return the selected gun state
    {
        foreach(var item in weaponGoList) if (item.name == itemEquip.ToString()) return item.activeSelf;
        return false;
    }
    #endregion

    #region - Character Inventory View -
    private void Update() => SetCharacterRotation();//This method control the player model rotation in the inventory player view using as base an slider value
    private void SetCharacterRotation() => characterObject.localRotation = Quaternion.Lerp(characterObject.localRotation, Quaternion.Euler(0, characterRotationSlider.value, 0), itemRotationSpeed * Time.deltaTime);
    #endregion
}