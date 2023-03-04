using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBag", menuName = "InventorySystem/Store Items/New Bag")]
public class BagScriptable : GenericBagScriptable
{
    #region - Data Declaration -
    [SerializeField, Range(1, 10)] protected int maxShortCutSlot;

    [SerializeField] private List<RulerScriptable> rulerList;

    [SerializeField] protected Dictionary<int, GenericItemScriptable> itemsShortCutDictionary;
    #endregion

    #region - Get and Set Data -
    public Dictionary<int, GenericItemScriptable> ItemsShortCutDictionary { get => itemsShortCutDictionary;}
    public int MaxShortCutSlot { get => maxShortCutSlot; }
    #endregion

    #region - Methods -
    protected override void OnEnable()
    {
        base.OnEnable();
        itemsShortCutDictionary = new Dictionary<int, GenericItemScriptable>();
    }
    protected override void ResetBag()
    { 
        base.ResetBag();
        matrixUtility = new MatrixUtility(maxRow, maxColumn, title);
    }
    public override bool UseItem(int id, int value) => base.UseItem(id, value);
    public bool AddItemToShortCut(int index, GenericItemScriptable item)
    {
        if (itemsShortCutDictionary.ContainsValue(item)) Debug.LogWarning("The item " + item.name + "is already on shortcuts!");
        else
        {
            if (CheckAllRules(index, item))
            {
                itemsShortCutDictionary.Add(index, item);
                return true;
            }
        }
        return false;
    }
    public bool ChangeItemPosition(GenericItemScriptable item, int index)
    {
        if (itemsShortCutDictionary.ContainsValue(item))
        {
            RemoveItemFromShortCutById(item.Id);
            AddItemToShortCut(index, item);
            return true;
        }
        return false;
    }
    public List<int> GetIdsFromItemShortCutDictionary()
    {
        List<int> resultIds = new List<int>();

        foreach(var item in itemsShortCutDictionary) resultIds.Add(item.Value.Id);

        return resultIds;
    }
    public List<int> GetUsedKeysFromShortCutDictionary()
    {
        List<int> resultKeys = new List<int>();
        
        foreach(var item in itemsShortCutDictionary) resultKeys.Add(item.Key);

        return resultKeys;
    }
    public GenericItemScriptable GetItemByIndexPosition(int index)
    {
        try
        {
            var result = itemsShortCutDictionary.First(element => element.Key == index);

            if (!(result.Value.Equals(null)))
            {
                return result.Value;
            }
        }
        catch(Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
        return null;
    }
    public int GetIndexByItem(GenericItemScriptable item)
    {
        int resultIndex = -1;
        try
        {
            var result = itemsShortCutDictionary.First(element => element.Value == item);

            if (!(result.Equals(null)))
            {
                resultIndex = result.Key;
                return resultIndex;
            }
        }
        catch(Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
        return resultIndex;
    }
    public bool RemoveItemFromShortCutById(int id)
    {
        try
        {
            var resultItem = itemsShortCutDictionary.First(element => element.Value.Id == id);

            if (!(resultItem.Equals(null)))
            {
                itemsShortCutDictionary.Remove(resultItem.Key);
                return true;
            }
        }
        catch(Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
        return false;
    }
    private bool CheckAllRules(int index, GenericItemScriptable item)
    {
        if (rulerList.Count != 0) foreach (var ruler in rulerList) if (ruler.Validade(index, item)) return true;
        else Debug.LogWarning("Thre is no Rules, so no item is allowed!");
        return false;
    }
    #endregion
}