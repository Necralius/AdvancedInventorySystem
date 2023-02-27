using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRuler", menuName = "InventorySystem/Filters/NewRulers")]
public class RulerScriptable : ScriptableObject
{
    #region - Data Declaration -
    [SerializeField, Range(1, 100)] private int idSelected;

    [SerializeField] private bool IgnoreAllSlotFilter;

    [SerializeField] private bool useIdAndIgonoreAllItemTypeFilter;

    [SerializeField] private SlotFilterScriptable slotFilter;

    [SerializeField] private ItemTypeFilterScriptable itemTypeFilter;

    #endregion


    #region - Methods -
    public bool Validade(int index, GenericItemScriptable item)
    {
        try
        {
            List<int> resultIndexList = slotFilter.GetAllIndex();
            List<ItemType> resultItemTypeList = itemTypeFilter.ItemType;

            if (resultItemTypeList.Count == 0)
            {
                Debug.LogWarning("There is no ItemType in ItemTypeFilter");
            }

            if (resultIndexList.Contains(index) || IgnoreAllSlotFilter)
            {
                if (!useIdAndIgonoreAllItemTypeFilter)
                {
                    foreach (var itemTypeInList in resultItemTypeList)
                    {
                        if (itemTypeInList == item.GetItemType())
                        {

                            return true;
                        }
                    }
                }
                else
                {
                    if (idSelected == item.Id)
                    {
                        return true;
                    }
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