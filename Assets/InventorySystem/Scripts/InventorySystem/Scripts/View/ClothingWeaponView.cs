using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] private ClothingWeaponScriptable currentClothingWeapon;

    private Dictionary<int, GameObject> specialSlotDictionary;
    public GameObject[] itensList;
    #endregion

    #region - Methods -

    public void Iniciate(ClothingWeaponScriptable CurrentClothingWeapon)
    {
        this.currentClothingWeapon = CurrentClothingWeapon;
        IniciateClothingWeaponSlots(currentClothingWeapon.SlotNumber);
    }

    private void IniciateClothingWeaponSlots(int clothingWeaponSlots)
    {
        specialSlotDictionary = new Dictionary<int, GameObject>();

        GameObject[] resultList = itensList;

        SimpleSlotView[] simpleSlotViewResult = new SimpleSlotView[resultList.Length];

        for (int i = 0; i < resultList.Length; i++) simpleSlotViewResult[i] = resultList[i].GetComponentInChildren<SimpleSlotView>();

        for (int column = 0; column < clothingWeaponSlots; column++) if (!(resultList[column].Equals(null))) simpleSlotViewResult[column].coordinate = new Vector2(0, column);

        itensList = GameObject.FindGameObjectsWithTag("ClothingWeaponsSlot"); //-> Debug

        int index = 0;
        foreach(var specialSlotGo in resultList)
        {
            specialSlotDictionary.Add(index, specialSlotGo);
            specialSlotDictionary[index].GetComponent<DisplayItemBehaviorView>().TurnOff();
            index++;
        }
    }
    public void RefreshSlotSystem(Dictionary<int, GenericItemScriptable> itemsClothingWeaponsDictionary)
    {
        foreach(var item in specialSlotDictionary)
        {
            item.Value.GetComponent<DisplayItemBehaviorView>().TurnOff();
        }

        if (!(itemsClothingWeaponsDictionary.Equals(null)))
        {
            foreach(var item in itemsClothingWeaponsDictionary)
            {
                GameObject resultGo = SelectSpecialSlotByIndex(item.Key);
                resultGo.GetComponent<DisplayItemBehaviorView>().TurnOn();
                resultGo.GetComponentInChildren<ComplexSlotView>().ItemView = item.Value;
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateText();

            }
        }
    }
    private GameObject SelectSpecialSlotByIndex(int index)
    {
        try
        {
            var result = specialSlotDictionary.First(element => element.Key == index);

            if (!(result).Equals(null))
            {
                return result.Value;
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning("No special Slot There");
        }
        return null;
    }
    #endregion


}