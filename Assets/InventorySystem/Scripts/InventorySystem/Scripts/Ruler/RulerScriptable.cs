using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRuler", menuName = "InventorySystem/Filters/NewRulers")]
public class RulerScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //RulerScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Ruler Data Declaration -
    [SerializeField, Range(1, 100)] private int idSelected;

    [SerializeField] private bool IgnoreAllSlotFilter;

    [SerializeField] private bool useIdAndIgonoreAllItemTypeFilter;

    [SerializeField] private SlotFilterScriptable slotFilter;

    [SerializeField] private ItemTypeFilterScriptable itemTypeFilter;
    #endregion

    #region - Methods -
    public bool Validade(int index, GenericItemScriptable item)//This method validate the item and his index using the current ruler rule and filter
    {
        try
        {
            List<int> resultIndexList = slotFilter.GetAllIndex();
            List<ItemType> resultItemTypeList = itemTypeFilter.ItemType;

            if (resultItemTypeList.Count == 0) Debug.LogWarning("There is no ItemType in ItemTypeFilter");

            if (resultIndexList.Contains(index) || IgnoreAllSlotFilter)
            {
                if (!useIdAndIgonoreAllItemTypeFilter) foreach (var itemTypeInList in resultItemTypeList) if (itemTypeInList == item.GetItemType()) return true;
                else
                {
                    if (idSelected == item.Id) return true;
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log("There is no Filters in SlotFilter / ItemTypeFilter or both of them");
        }
        return false;
    }
    #endregion
}