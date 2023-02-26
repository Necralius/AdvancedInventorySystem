using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static GameManager Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Inventory Change -
    public GameObject inventoryUIObject;
    public bool inventoryIsOpen = false;
    public void ChangeInventoryState()
    {
        inventoryUIObject.SetActive(!inventoryUIObject.activeInHierarchy);
        inventoryIsOpen = inventoryUIObject.activeInHierarchy;
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        InventoryManagerController.Instance.InventoryStateChanged();
    }
    #endregion
    private void Start()
    {
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}