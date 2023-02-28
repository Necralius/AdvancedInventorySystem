using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingWeaponView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static ClothingWeaponView Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -

    [SerializeField] private GameObject ClothingWeaponsGo;
    [SerializeField] private GameObject ClothingWeaponSpecialGroup;
    [SerializeField] private GameObject ClothingWeaponSpecialSlot;

    [SerializeField] private GameObject auxCameraPreviewGo;

    [HideInInspector] public ClothingWeaponScriptable currentClothingWeapon;

    private Dictionary<int, GameObject> specialSlotDictionary;
    #endregion

    #region - Methods -

    public void Iniciate(ClothingWeaponScriptable currentClothingWeapon)
    {
        
    }

    private void IniciateClothingWeaponSlots(List<GameObject> specialSlots)
    {
        
    }

    public bool ShowAndHide()
    {
        ClothingWeaponsGo.SetActive(!ClothingWeaponsGo.activeInHierarchy);
        CallSpinRoutine();
        return ClothingWeaponsGo.activeInHierarchy;
    }
    private void CallSpinRoutine()
    {
        //Rotate the player Image in the menu
    }

    #endregion


}