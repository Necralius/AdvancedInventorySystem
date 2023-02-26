using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericItemScriptable : ScriptableObject
{
    #region - Main Declaration -

    [SerializeField] private int id;

    [SerializeField] private Sprite icon;

    [SerializeField] private bool isDroppable;
    [SerializeField] private bool removeWhenNumberIsZero;
    [SerializeField] private bool isOnlyItem;

    [SerializeField] private string label;
    [SerializeField,TextArea] private string description;

    [SerializeField] private int currentQuantity;
    [SerializeField] private int maxQuantity;

    [SerializeField, Range(1, 6)] private int slotSize = 1;

    [SerializeField] private float itemWeigth;
    [SerializeField] private float totalWeightPerItem;

    [SerializeField] private List<GenericActionScriptable> actionUseList;
    
    protected ActionManagerEvent actionManagerEvnt;
    
    #endregion

    #region - Get and Set Data -
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

    #region - Item Methods -
    public void OnEnable()
    {
        Reset();
        UpdateWeight();
    }
    public void Reset()
    {
        currentQuantity = 0;
    }
    public bool Add(int value)
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
                currentQuantity += value; UpdateWeight();
                return true;
            }
        }
        return false;
    }
    private bool Subtract(int value)
    {
        if (currentQuantity - value >= 0)
        {
            currentQuantity -= value; UpdateWeight();
            return true;
        }
        return false;
    }
    public virtual bool Use(int value)
    {
        if (IsOnlyItem)
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
    public virtual void ActionUseListDispatch()
    {
        actionManagerEvnt = new ActionManagerEvent();

        actionManagerEvnt.DispatchAllGenericActionListEvent(actionUseList);


    }
    public virtual void ActionEquipandUnequipListDispatch()
    {

    }

    void UpdateWeight() => totalWeightPerItem = itemWeigth * currentQuantity;
    #endregion

}