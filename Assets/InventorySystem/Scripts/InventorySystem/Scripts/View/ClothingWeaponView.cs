using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ClothingWeaponView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //ClothingWeaponView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static ClothingWeaponView Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Main Data Declaration -
    [SerializeField] private GameObject ClothingWeaponsGameobject;
    [SerializeField] private GameObject ClothingWeaponSpecialGroup;
    [SerializeField] private GameObject ClothingWeaponSpecialSlot;

    [SerializeField] private ClothingWeaponScriptable currentClothingWeapon;

    private Dictionary<int, GameObject> specialSlotDictionary;
    public GameObject[] itensList;
    #endregion

    //=========== Method Area ===========//

    #region - Clothing Slot Start -
    public void Iniciate(ClothingWeaponScriptable CurrentClothingWeapon)//This method iniciate the ClothingWeapon Slots on the the method call
    {
        this.currentClothingWeapon = CurrentClothingWeapon;
        IniciateClothingWeaponSlots(currentClothingWeapon.SlotNumber);
    }
    private void IniciateClothingWeaponSlots(int clothingWeaponSlots)//This method get all the ClothingWeapon Slots in the scene and manage them
    {
        specialSlotDictionary = new Dictionary<int, GameObject>();

        GameObject[] resultList = itensList;

        SimpleSlotView[] simpleSlotViewResult = new SimpleSlotView[resultList.Length];

        for (int i = 0; i < resultList.Length; i++) simpleSlotViewResult[i] = resultList[i].GetComponentInChildren<SimpleSlotView>();

        for (int column = 0; column < clothingWeaponSlots; column++) if (!(resultList[column].Equals(null))) simpleSlotViewResult[column].coordinate = new Vector2(0, column);//This statement organize the slots using the correct coordinates

        //itensList = GameObject.FindGameObjectsWithTag("ClothingWeaponsSlot"); //This statement show the complete Clothing Weapon Slot list, is used only for visual debug

        int index = 0;
        foreach(var specialSlotGo in resultList)//This statements setup the Clothing Slots on the correct dictionary
        {
            specialSlotDictionary.Add(index, specialSlotGo);
            specialSlotDictionary[index].GetComponent<DisplayItemBehaviorView>().TurnOff();
            index++;
        }
    }
    #endregion

    #region - Clothing Weapon Slot 
    public void RefreshSlotSystem(Dictionary<int, GenericItemScriptable> itemsClothingWeaponsDictionary)//This method refresh the Slot System in the Clothing Slots
    {
        foreach(var item in specialSlotDictionary) item.Value.GetComponent<DisplayItemBehaviorView>().TurnOff();

        if (!(itemsClothingWeaponsDictionary.Equals(null)))
        {
            foreach(var item in itemsClothingWeaponsDictionary)//This statements setup all the Slot System functionalities, this set the slot as active, change his icon, text and itemView Value 
            {
                GameObject resultGo = SelectSpecialSlotByIndex(item.Key);
                resultGo.GetComponent<DisplayItemBehaviorView>().TurnOn();
                resultGo.GetComponentInChildren<ComplexSlotView>().ItemView = item.Value;
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateText();
            }
        }
    }
    private GameObject SelectSpecialSlotByIndex(int index)//This method returns an ClothingWeapon slot based on his index
    {
        try
        {
            var result = specialSlotDictionary.First(element => element.Key == index);

            if (!(result).Equals(null)) return result.Value;
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning("No special Slot There");
        }
        return null;
    }
    #endregion
}