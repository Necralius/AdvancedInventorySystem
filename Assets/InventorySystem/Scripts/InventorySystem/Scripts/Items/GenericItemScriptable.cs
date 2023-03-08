using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericItemScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //GenericItemScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -

    [SerializeField] private int id;

    [SerializeField] private Sprite icon;

    [SerializeField] private bool isDroppable;
    [SerializeField] private bool removeWhenNumberIsZero;
    [SerializeField] private bool isOnlyItem;

    [SerializeField] private string label;
    [SerializeField,TextArea] private string description;

    [SerializeField] private int currentQuantity;
    [SerializeField] private int maxQuantity;

    [SerializeField, Range(1, 8)] private int slotSize = 1;

    [SerializeField] private float itemWeigth;
    [SerializeField] private float totalWeightPerItem;

    [SerializeField] private List<GenericActionScriptable> actionUseList;
    
    protected ActionManagerEvent actionManagerEvnt;

    #endregion

    #region - Get and Set Data -
    //This statements protect and manage the item data setting and get
    public int Id { get => id; }
    public abstract ItemType GetItemType();
    public Sprite Icon { get => icon; }
    public bool IsDroppable { get => isDroppable;}
    public bool RemoveWhenNumberIsZero { get => removeWhenNumberIsZero;}
    public bool IsOnlyItem { get => isOnlyItem;}
    public string Label { get => label;}
    public string Description { get => description;}
    public int CurrentQuantity { get => currentQuantity;}
    public int MaxQuantity { get => maxQuantity; }
    public int SlotSize { get => slotSize; }
    public float ItemWeigth { get => itemWeigth; }
    public float TotalWeightPerItem
    {
        get { UpdateWeight(); return totalWeightPerItem; }
    }
    #endregion

    #region - Item Managment -
    public void OnEnable()//This method reset all the scriptable asset on the game enable
    {
        Reset();
        UpdateWeight();
    }
    public void Reset() => currentQuantity = 0;//this method reset the item quantity

    #region - Item Add -
    public bool Add(int value)//This method add an item quantity to the current item values
    {
        if (isOnlyItem)
        {
            currentQuantity = 1;
            return true;
        }
        else
        {
            if (value + currentQuantity <= maxQuantity)
            {
                currentQuantity += value; UpdateWeight();//This statements update the item quantity and weight
                return true;
            }
        }
        return false;
    }
    #endregion

    #region - Item Subtract -
    private bool Subtract(int value)//This method represent the item subtraction
    {
        if (currentQuantity - value >= 0)
        {
            currentQuantity -= value; UpdateWeight();
            return true;
        }
        return false;
    }
    #endregion

    #region - Item Use -
    public virtual bool Use(int value)//This method represent the item use mechanic, also the method is overridable
    {
        if (IsOnlyItem)//If the item is unique, the method send the action list to the manager
        {
            ActionUseListDispatch();
            return true;
        }
        else
        {
            if (Subtract(value))
            {
                ActionUseListDispatch();
                return true;
            }
        }
        return false;
    }
    #endregion

    #region - Action Management -
    public virtual void ActionUseListDispatch()//This method represents the item generic action pass to the ActionManagerEvent class
    {
        actionManagerEvnt = new ActionManagerEvent();
        actionManagerEvnt.DispatchAllGenericActionListEvent(actionUseList);
    }
    public virtual void ActionEquipandUnequipListDispatch() { }//This method represents an overridable method functionality 
    #endregion

    #region - Item Weight Calculation -
    void UpdateWeight() => totalWeightPerItem = itemWeigth * currentQuantity;//This method calculates and update the item weight considering his quantity
    #endregion

    #endregion
}