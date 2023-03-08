using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlotFilter", menuName = "InventorySystem/Filters/NewSlotFilter")]
public class SlotFilterScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //SlotFilterScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Data Declaration 
    [SerializeField, Range(0, 9)] private int indexFrom;

    [SerializeField, Range(0, 9)] private int indexTo;

    #endregion

    #region - Slot Filter Managment -
    private void OnEnable()//This method verifies and limit the item index filter value
    {
        if (indexFrom > indexTo) indexFrom = indexTo;
    }
    #endregion

    #region - Slot Filter Production -
    public List<int> GetAllIndex()//This method make and list of int that consider the passed range on the SlotFilter data declaration and return it
    {
        List<int> indexRange = new List<int>();

        if (indexFrom - indexTo != 0) for (int i = 0; i < indexTo; i++) indexRange.Add(i);
        else indexRange.Add(indexFrom);
        return indexRange;
    }
    #endregion
}