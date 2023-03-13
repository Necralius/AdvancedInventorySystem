using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //GameManager - Code Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statements means a simple Singleton Pattern implementation
    public static GameManager Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Key Code Database -
    public KeyCodeGroup GeneralKeyCodes;
    #endregion

    #region - Main Data Declaration - 
    public TextMeshProUGUI restartScene;
    #endregion

    #region - BuildIn Methods -
    private void Update()
    {
        if (Input.GetKeyDown(GeneralKeyCodes.GetKeyCodeByName("RestartSceneKey")))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//This statement restarts the scene if the selected key is pressed
            InventoryManagerController.Instance.currentBag.ResetBag();
            InventoryManagerController.Instance.currentClothingWeapon.ResetSlots();
            InventoryManagerController.Instance.CastGenericBagToBag().ResetShortCut();
        }
    }
    private void Start()
    {
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;//This method automatcally set the cursor lock state based in the current inventory state
        restartScene.text = "Press " + GeneralKeyCodes.GetKeyCodeByName("RestartSceneKey").ToString() + " to restart the scene";
    }
    #endregion

    #region - Inventory Change -
    public GameObject inventoryUIObject;
    public bool inventoryIsOpen = false;
    public GameObject hudObj;

    public void ChangeInventoryState()//This method represents the inventory transiction between activated and not activated
    {
        inventoryUIObject.SetActive(!inventoryUIObject.activeInHierarchy);
        inventoryIsOpen = inventoryUIObject.activeInHierarchy;
        Cursor.lockState = inventoryIsOpen ? CursorLockMode.None : CursorLockMode.Locked;
        hudObj.SetActive(!inventoryIsOpen);
        InventoryManagerController.Instance.InventoryStateChanged();
        InventoryManagerController.Instance.CallRefreshClothingWeaponView();
    }
    #endregion
}