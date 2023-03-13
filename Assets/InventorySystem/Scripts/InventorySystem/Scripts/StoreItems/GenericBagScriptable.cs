using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class GenericBagScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //GenericBagScriptable - Code Update Version 0.4 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -
    private bool usedOrganizeBtSizePriority = false;

    [SerializeField] private Sprite icon;

    [SerializeField] protected string title;

    [SerializeField, Range(1, 10)] protected int maxColumn;
    [SerializeField, Range(1, 10)] protected int maxRow;

    [SerializeField] protected int currentSlotUse;

    [SerializeField, Range(0.1f, 100f)] protected float weightLimited;
    [SerializeField] protected float currentWeightUse;

    [SerializeField] protected bool autoOrganize;

    [SerializeField] protected List<GenericItemScriptable> itemList;

    protected MatrixUtility matrixUtility;
    #endregion

    #region - Get and Set Data -
    //This statements protect and manage the item data setting and get
    public Sprite Icon { get => icon; }
    public string Title { get => title; }
    public int MaxColumn { get => maxColumn;}
    public int MaxRow { get => maxRow; }
    public int SlotLimited { get => maxRow * maxColumn; }
    public int CurrentSlot { get => currentSlotUse; }
    public float WeightLimited { get => weightLimited; }
    public bool UsedOrganizeBtSizePriority { get => usedOrganizeBtSizePriority; set => usedOrganizeBtSizePriority = value; }
    public float CurrentWeightUse { get => currentWeightUse; set => currentWeightUse = value; }
    #endregion

    //=========== Method Area ===========//

    #region - Bag Reset -
    protected virtual void OnEnable() => ResetBag();//This deactivate the persistent data for the inventory system
    public virtual void ResetBag()//This method reset the bag items and its aspects
    {
        itemList = new List<GenericItemScriptable>();
        currentSlotUse = 0;
        CurrentWeightUse = 0;
    }
    #endregion

    #region - Item Slot Capacity Validation -
    protected virtual bool SlotCapacityValidation(GenericItemScriptable item)
    {
        if (item.SlotSize == 2 || item.SlotSize == 3 || item.SlotSize == 5) if (item.SlotSize > maxColumn && item.SlotSize > maxRow) return false;
        return true;
    }
    #endregion

    #region - Item Validation -
    protected virtual bool SizeWeightNumberValidation(GenericItemScriptable item, int number, bool isNewItem)//This method validate bag slot size and weight
    {
        if (isNewItem)//This statment calculates the item slot size and weight considering that the item is new in the bag
        {
            if (CurrentSlot + item.SlotSize <= SlotLimited)
            {
                if (CurrentWeightUse + item.TotalWeightPerItem * number <= weightLimited)
                {
                    if (item.Add(number))
                    {
                        UpdateSizeAndWeight();
                        return true;
                    }
                    else Debug.LogWarning("Over Number!");
                }
                else Debug.LogWarning("Over weight Limited!");
            }
            else Debug.LogWarning("Over Slot size Limit!");
        }
        else//This statment calculates the item slot size and weight considering that the item already exists on the bag
        {
            if (CurrentWeightUse + item.TotalWeightPerItem * number <= weightLimited)
            {
                if (item.Add(number))
                {
                    UpdateSizeAndWeight();
                    return true;
                }
            }
            else Debug.LogWarning("Weight is Over Limit!");
        }
        return false;
    }
    #endregion

    #region - Item Size and Weight Update -
    protected virtual void UpdateSizeAndWeight()//This method update the bag size and weight based on his quantity
    {
        currentSlotUse = 0;
        CurrentWeightUse = 0;

        foreach(var item in itemList)
        {
            currentSlotUse += item.SlotSize;
            CurrentWeightUse += item.TotalWeightPerItem;
        }
    }
    #endregion

    #region - Item Addd Functionality -
    public virtual bool AddItem(GenericItemScriptable item, int number)//This method adds the selected item on the bag
    {
        if (itemList.Exists((GenericItemScriptable itemFromList) => itemFromList.Id == item.Id))//This statement verifies if the item exists on the bag
        {
            if (SizeWeightNumberValidation(item, number, false)) return true;//This statement verifies if the item can be added to the bag, considering the bag weight limitations, and the item number limit
        }
        else
        {
            if (!SlotCapacityValidation(item)) return false;

            List<Vector2> listResult = new List<Vector2>();
            listResult = matrixUtility.LookForFreeArea(item.SlotSize);//This statement make the Matrix Utility Class search for avaliable space in the grid to add the item

            if (listResult.Count > 0)
            {
                if (SizeWeightNumberValidation(item, number, true))//If has enough space in the grid, the bag verifies if the item can be added considering the item slot and weight
                {
                    matrixUtility.SetItem(listResult, item.Id);
                    itemList.Add(item);
                    UpdateSizeAndWeight();
                    return true;
                }
            }
            else
            {
                if (SizeWeightNumberValidation(item, number, true))//If do not has enough space in the grid the inventory automatically organize itself 
                {
                    Debug.LogWarning("There is no room next! maybe the inventory must be oganized!");

                    if (autoOrganize)
                    {
                        itemList.Add(item);
                        UpdateSizeAndWeight();
                        OrgnizeBySizePriority();                 
                    }
                }
                else Debug.LogWarning("There is no more slots!");
            }
        }
        return false;
    }
    #endregion

    #region - Item Use -
    public virtual bool UseItem(int id, int value)//This method represent the item usage and update on the inventory
    {
        GenericItemScriptable item = FindItemById(id);
        if (item != null)
        {
            if (item.Use(value))
            {
                UpdateSizeAndWeight();
                return true;
            }
            else Debug.LogWarning("There is no quantity enogh");
        }
        else Debug.LogWarning("The item cannot be used!");
        return false;
    }
    #endregion

    #region - Item Removal System -
    public bool RemoveItem(int id)//This method represent the item removal from inventory using as instruction the item id
    {
        GenericItemScriptable item = FindItemById(id);
        if (item != null)
        {
            matrixUtility.ClearItemOnMatrix(id);//This statement uses the Matrix Utility to clear the used space by the item
            itemList.Remove(item);
            UpdateSizeAndWeight();
            return true;
        }
        return false;
    }
    public bool RemoveItem(GenericItemScriptable item)//This method represent the item removal from inventory using as instruction the item object itself
    {
        if (item != null)
        {
            matrixUtility.ClearItemOnMatrix(item.Id);
            itemList.Remove(item);
            UpdateSizeAndWeight();
            return true;
        }
        return false;
    }
    #endregion

    #region - Item Drop -
    public bool DropItem(int id)//This method represent the item drop functionality 
    {
        GenericItemScriptable item = FindItemById(id);
        if (item != null)
        {
            if (item.IsDroppable)
            {
                item.Reset();
                RemoveItem(item);
                return true;
            }
        }
        return true;
    }
    #endregion

    #region - Item Organization by Size -
    public virtual void OrgnizeBySizePriority()//This method organize the item list prioritazing the item slot size
    {
        usedOrganizeBtSizePriority = true;
        List<GenericItemScriptable> temporaryList = itemList.OrderByDescending(x => x.SlotSize).ToList();//This statement make an temporary and reorganized list that use the item SlotSize as 

        ResetBag();//This statment resets the bag
        matrixUtility.PopulateMatrix();//This statment uses the Matrix Utility class to populate the matrix with the new list organized by size

        foreach(GenericItemScriptable item in temporaryList) AddItem(item, 0);
    }
    #endregion

    #region - Item and Space Search -
    public List<Vector2> FindCellById(int id) => matrixUtility.FindLocationById(id);//This method use the Matrix Utility class to return an certain location
    public GenericItemScriptable FindItemById(int id) => itemList.Find(obj => obj.Id == id);//This method returns an item from the bag using as identificator the item id
    public List<GenericItemScriptable> ReturnFullList() => itemList;//This method return the full item list
    #endregion
}