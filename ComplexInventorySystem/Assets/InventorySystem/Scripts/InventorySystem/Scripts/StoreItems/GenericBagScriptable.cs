using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class GenericBagScriptable : ScriptableObject
{
    #region - Main Declaration
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
    public Sprite Icon { get => icon; }
    public string Title { get => title; }
    public int MaxColumn { get => maxColumn;}
    public int MaxRow { get => maxRow; }
    public int SlotLimited { get => maxRow * maxColumn; }
    public int CurrentSlot { get => currentSlotUse; }
    public float WeightLimited { get => weightLimited; }
    public bool UsedOrganizeBtSizePriority { get => usedOrganizeBtSizePriority; set => usedOrganizeBtSizePriority = value; }
    #endregion

    #region - Methods -

    protected virtual void OnEnable()
    {
        ResetBag();//This deactivate the persistent data for the inventory system, to make the items save correcly, just coment this statment!
    }
    protected virtual void ResetBag()
    {
        itemList = new List<GenericItemScriptable>();
        currentSlotUse = 0;
        currentWeightUse = 0;
    }

    protected virtual bool SlotCapacityValidation(GenericItemScriptable item)
    {
        if (item.SlotSize == 2 || item.SlotSize == 3 || item.SlotSize == 5)
        {
            if (item.SlotSize > maxColumn && item.SlotSize > maxRow)
            {
                return false;

            }
            else return true;
        }
        return true;
    }

    protected virtual bool SizeWeightNumberValidation(GenericItemScriptable item, int number, bool isNewItem)
    {
        if (isNewItem)
        {
            if (CurrentSlot + item.SlotSize <= SlotLimited)
            {
                if (currentWeightUse + item.TotalWeightPerItem * number <= weightLimited)
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
        else
        {
            if (currentWeightUse + item.TotalWeightPerItem * number <= weightLimited)
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
    protected virtual void UpdateSizeAndWeight()
    {
        currentSlotUse = 0;
        currentWeightUse = 0;

        foreach(var item in itemList)
        {
            currentSlotUse += item.SlotSize;
            currentWeightUse += item.TotalWeightPerItem;

        }
    }
    public virtual bool AddItem(GenericItemScriptable item, int number)
    {
        //Item Exists

        if (itemList.Exists((GenericItemScriptable itemFromList) => itemFromList.Id == item.Id))
        {
            //SlotSize
            //Weigth
            //Number
            if (SizeWeightNumberValidation(item, number, false))
            {

                return true;
            }
        }
        else
        {
            if (!SlotCapacityValidation(item)) return false;

            List<Vector2> listResult = new List<Vector2>();
            listResult = matrixUtility.LookForFreeArea(item.SlotSize);

            if (listResult.Count > 0)
            {
                if (SizeWeightNumberValidation(item, number, true))
                {
                    matrixUtility.SetItem(listResult, item.Id);
                    itemList.Add(item);
                    UpdateSizeAndWeight();
                    return true;
                }
            }
            else
            {
                if (SizeWeightNumberValidation(item, number, true))
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
        //Item Don't Exists
        return false;
    }

    public virtual bool UseItem(int id, int value)
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

    public bool RemoveItem(int id)
    {
        GenericItemScriptable item = FindItemById(id);
        if (item != null)
        {
            matrixUtility.ClearItemOnMatrix(id);
            itemList.Remove(item);
            UpdateSizeAndWeight();
            return true;
        }
        return false;
    }
    public bool RemoveItem(GenericItemScriptable item)
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
    public bool DropItem(int id)
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
    public virtual void OrgnizeBySizePriority()
    {
        usedOrganizeBtSizePriority = true;
        List<GenericItemScriptable> temporaryList = itemList.OrderByDescending(x => x.SlotSize).ToList();

        ResetBag();
        matrixUtility.PopulateMatrix();

        foreach(GenericItemScriptable item in temporaryList)
        {
            AddItem(item, 0);
        }
    }

    public List<Vector2> FindCellById(int id) => matrixUtility.FindLocationById(id);

    public GenericItemScriptable FindItemById(int id) => itemList.Find(obj => obj.Id == id);

    public List<GenericItemScriptable> ReturnFullList() => itemList;
    #endregion
}