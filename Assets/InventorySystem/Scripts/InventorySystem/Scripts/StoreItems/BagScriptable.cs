using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBag", menuName = "InventorySystem/Store Items/New Bag")]
public class BagScriptable : GenericBagScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //BagScriptable - Code Update Version 0.4 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -
    [SerializeField, Range(1, 10)] protected int maxShortCutSlot;

    [SerializeField] private List<RulerScriptable> rulerList;

    [SerializeField] protected Dictionary<int, GenericItemScriptable> itemsShortCutDictionary;
    #endregion

    #region - Get and Set Data -
    //This statements protect and manage the item data setting and get
    public Dictionary<int, GenericItemScriptable> ItemsShortCutDictionary { get => itemsShortCutDictionary;}
    public int MaxShortCutSlot { get => maxShortCutSlot; }
    #endregion

    //=========== Method Area ===========//

    #region - Bag Start -
    protected override void OnEnable()//This method reset and start the bag functionalities
    {
        base.OnEnable();
        itemsShortCutDictionary = new Dictionary<int, GenericItemScriptable>();
    }
    #endregion

    #region - Bag Reset Functionality -
    protected override void ResetBag()//This method overrides the bag reset method
    { 
        base.ResetBag();
        matrixUtility = new MatrixUtility(maxRow, maxColumn, title);
    }
    #endregion

    #region - Bag Use Functionality -
    public override bool UseItem(int id, int value) => base.UseItem(id, value);//This method override the default use item method
    public bool AddItemToShortCut(int index, GenericItemScriptable item)//This method adds an item to an shortcut slot
    {
        if (itemsShortCutDictionary.ContainsValue(item)) Debug.LogWarning("The item " + item.name + "is already on shortcuts!");
        else
        {
            if (CheckAllRules(index, item))//This statement verifies the bag shortcut slot rules
            {
                itemsShortCutDictionary.Add(index, item);
                return true;
            }
        }
        return false;
    }
    #endregion

    #region - Shortcut System -

    #region - ShortCut Position Change -
    public bool ChangeItemPosition(GenericItemScriptable item, int index)//This method changes the item shortcut position from one shortcut to another
    {
        if (itemsShortCutDictionary.ContainsValue(item))
        {
            RemoveItemFromShortCutById(item.Id);
            AddItemToShortCut(index, item);
            return true;
        }
        return false;
    }
    #endregion

    #region - ShortCut Search by Dictionary -
    public List<int> GetIdsFromItemShortCutDictionary()//This method returns all the items ids from the shortcut dictionary as a list
    {
        List<int> resultIds = new List<int>();

        foreach(var item in itemsShortCutDictionary) resultIds.Add(item.Value.Id);

        return resultIds;
    }
    public List<int> GetUsedKeysFromShortCutDictionary()//This method returns all the items key value from the shortcut dictionary as a list
    {
        List<int> resultKeys = new List<int>();
        
        foreach(var item in itemsShortCutDictionary) resultKeys.Add(item.Key);

        return resultKeys;
    }
    #endregion

    #region - Item Search -
    public GenericItemScriptable GetItemByIndexPosition(int index)//This method returns the item that exists considering the item index passed on the method armument
    {
        //This method also uses the Try Catch block to to avoid total code break
        try
        {
            var result = itemsShortCutDictionary.First(element => element.Key == index);

            if (!(result.Value.Equals(null))) return result.Value;
        }
        catch(Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
        return null;
    }
    public int GetIndexByItem(GenericItemScriptable item)//This method returns the item index from shortcut slot
    {
        //This method also uses the Try Catch block to to avoid total code break
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
    #endregion

    #region - Item Removal From ShortCut -
    public bool RemoveItemFromShortCutById(int id)//This method remove the item founded in the shortcuts based in tem item id
    {
        //This method also uses the Try Catch block to to avoid total code break
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
    #endregion

    #region - Item Rule Validation -
    private bool CheckAllRules(int index, GenericItemScriptable item)
    {
        if (rulerList.Count != 0) foreach (var ruler in rulerList) if (ruler.Validade(index, item)) return true;
        else Debug.LogWarning("Thre is no Rules, so no item is allowed!");
        return false;
    }
    #endregion

    #endregion
}