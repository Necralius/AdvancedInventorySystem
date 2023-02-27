using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlotFilter", menuName = "InventorySystem/Filters/NewSlotFilter")]
public class SlotFilterScriptable : ScriptableObject
{
    #region - Data Declaration 
    [SerializeField, Range(0, 9)] private int indexFrom;

    [SerializeField, Range(0, 9)] private int indexTo;

    #endregion

    #region - Methods -

    private void OnEnable()
    {
        if (indexFrom > indexTo) indexFrom = indexTo;
    }
    public List<int> GetAllIndex()
    {
        List<int> indexRange = new List<int>();

        if (indexFrom - indexTo != 0) for (int i = 0; i < indexTo; i++) indexRange.Add(i);
        else indexRange.Add(indexFrom);
        return indexRange;
    }
    #endregion

}