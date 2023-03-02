using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static PlayerView Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -

    [SerializeField] private List<GameObject> weaponGoList;

    #endregion

    #region - Methods -
    public void Equip(ItemEquip typeWeapon, bool visible)
    {
        foreach(var item in weaponGoList) if (item.name == typeWeapon.ToString()) item.SetActive(visible);
    }
    public bool StatusEquipment(ItemEquip itemEquip)
    {
        foreach(var item in weaponGoList) if (item.name == itemEquip.ToString()) return item.activeSelf;
        return false;
    }
    private void Start()
    {
        Equip(ItemEquip.Handable_FireGun_M4A4_Carbine, true);
    }
    #endregion
}