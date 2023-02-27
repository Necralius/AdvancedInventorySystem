using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewItemTypeFilter", menuName = "InventorySystem/Filters/NewItemTypeFilter")]
public class ItemTypeFilterScriptable : ScriptableObject
{
    #region - Data Declaration -
    [SerializeField] private List<ItemType> itemType;


    #endregion

    #region - Data Get and Set -

    public List<ItemType> ItemType { get => itemType; }
    #endregion

}