using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static GameManager Instance;
    void Awake() => Instance = this;
    #endregion

    public GameObject CrossHair;

    public KeyCodeGroup GeneralKeyCodes;

    #region - Inventory Change -
    public GameObject inventoryUIObject;
    public bool inventoryIsOpen = false;
    public GameObject hudObj;

    public void ChangeInventoryState()
    {
        inventoryUIObject.SetActive(!inventoryUIObject.activeInHierarchy);
        inventoryIsOpen = inventoryUIObject.activeInHierarchy;
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        hudObj.SetActive(!inventoryIsOpen);
        InventoryManagerController.Instance.InventoryStateChanged();
        InventoryManagerController.Instance.CallRefreshClothingWeaponView();
    }
    #endregion
    public void ChangeCrosHairState() => CrossHair.SetActive(!CrossHair.activeInHierarchy);
    private void Start()
    {
        //ChangeInventoryState();
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}