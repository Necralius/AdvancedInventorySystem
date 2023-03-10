using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShortCutView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ShortCutView - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static ShortCutView Instance;
    public void Awake() => Instance = this;
    #endregion

    #region - Main Data Declaration -
    [SerializeField] private GameObject shortCutGO;
    [SerializeField] private GameObject slotSpecialGroup;
    [SerializeField] private GameObject slotSpecialSlotGo;

    private Dictionary<int, GameObject> specialSlotDictionary;
    #endregion

    #region - Short Cut Iniciation -
    public void IniciateShortCutSlots(int maxShortSlot)//This method initiate the ShortCut Slots automatically
    {
        for (int column = 0; column < maxShortSlot; column++)//This statements automatically instantiate, configure and adds all the slots to the slots dictionary 
        {
            GameObject resultGo = Instantiate(slotSpecialSlotGo);
            SimpleSlotView[] resultSimpleSlotView = resultGo.GetComponentsInChildren<SimpleSlotView>();

            if (!(resultSimpleSlotView[0].Equals(null))) resultSimpleSlotView[0].coordinate = new Vector2(0, column);

            resultGo.transform.SetParent(slotSpecialGroup.transform);
        }
        specialSlotDictionary = new Dictionary<int, GameObject>();
        GameObject[] resultList = GameObject.FindGameObjectsWithTag("SpecialSlot");
        int index = 0;

        foreach(var item in resultList)
        {
            specialSlotDictionary.Add(index, item);
            index++;
        }
    }
    #endregion

    #region - Short Cut Slot Refresh -
    public void RefreshSlotSystem(Dictionary<int, GenericItemScriptable> itemsShortCutDictionary)//This method updates all the slot system UI based on a item dictionary passed on the arguments
    {
        foreach(var item in specialSlotDictionary) item.Value.GetComponent<DisplayItemBehaviorView>().TurnOff();

        if (!(itemsShortCutDictionary.Equals(null)))
        {
            foreach(var item in itemsShortCutDictionary)
            {
                GameObject resultGo = SelectedSpecialSlotByIndex(item.Key);
                resultGo.GetComponent<DisplayItemBehaviorView>().TurnOn();
                resultGo.GetComponentInChildren<ComplexSlotView>().ItemView = item.Value;
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateText();
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
            }
        }
    }
    public void UpdateSlot(Dictionary<int, GenericItemScriptable> itemsShortCutDictionary, List<int> usedKeys)//This method receive an slot dictionary and an especific keys list, later the method select all the items with the keys and update his UI
    {
        foreach (var key in usedKeys)
        {
            var resultSpecialSlotDictionary = specialSlotDictionary.First(element => element.Key == key);
            resultSpecialSlotDictionary.Value.GetComponent<DisplayItemBehaviorView>().TurnOn();

            var resultItemShortCutDictionary = itemsShortCutDictionary.First(element => element.Key == key);
            GenericItemScriptable resultItem = resultItemShortCutDictionary.Value;

            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().ItemView = resultItem;
            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().UpdateText();
            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
        }
    }
    #endregion

    #region - Slot Selection -
    private GameObject SelectedSpecialSlotByIndex(int id)//This method select an sepecial slot based on his id or key
    {
        try
        {
            var result = specialSlotDictionary.First(element => element.Key == id);

            if (!(result.Equals(null))) return result.Value;
        }
        catch(System.Exception ex)
        {
            Debug.LogException(ex);
        }
        return null;
    }
    #endregion
}