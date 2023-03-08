using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItemTypeFilter", menuName = "InventorySystem/Filters/NewItemTypeFilter")]
public class ItemTypeFilterScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ItemTypeFilterScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -
    [SerializeField] private List<ItemType> itemType;
    #endregion

    #region - Get and Set Data -
    //This statement protect and manage the item data setting and get
    public List<ItemType> ItemType { get => itemType; }
    #endregion

}