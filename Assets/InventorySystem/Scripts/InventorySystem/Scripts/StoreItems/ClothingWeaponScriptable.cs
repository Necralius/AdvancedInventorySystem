using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clothing Weapon", menuName = "InventorySystem/StoreItems/New Clothing Weapon")]
public class ClothingWeaponScriptable : ScriptableObject
{
    #region - Data Declaration
    [SerializeField, Range(1, 8)] private int slotNumber;

    [SerializeField, Range(1, 4)] private int gunSlots;
    [SerializeField, Range(1, 4)] private int clothingSlots;

    [SerializeField] private List<RulerScriptable> ruleList;

    [SerializeField] private Dictionary<int, GenericItemScriptable> itemsDictionary;

    [SerializeField] private float currentWeightUse;

    #endregion

    #region - Data Get and Set -
    public float CurrentWeightUse { get => currentWeightUse; }
    public Dictionary<int, GenericItemScriptable> ItemsDictionary { get => itemsDictionary; }
    public int SlotNumber { get => slotNumber; }
    public int GunSlots { get => gunSlots; }
    public int ClothingSlots { get => clothingSlots; }

    #endregion

    #region - Methods -
    private void OnEnable()
    {
        itemsDictionary = new Dictionary<int, GenericItemScriptable>();
        currentWeightUse = 0;
    }
    public bool AddItem(int index, GenericItemScriptable item)
    {
        if (itemsDictionary.ContainsValue(item))
        {
            Debug.LogWarning("The item: " + item.name + "Cannot be added, its already in the slot!");
            return false;
        }
        else
        {
            if (CheckAllRules(index, item))
            {
                ItemsDictionary.Add(index, item);
                UpdateTotalWeight();
            }
        }
        return false;
    }
    private bool CheckAllRules(int index, GenericItemScriptable item)
    {
        try
        {
            if (ruleList.Count != 0)
            {
                foreach(var rule in ruleList)
                {
                    if (rule.Validade(index, item)) return true;
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning("Do not Exists Rules in the list!");
        }
        return false;
    }
    public GenericItemScriptable GetItemById(int id)
    {
        var resultElement = itemsDictionary.First(element => element.Value.Id == id);
        return resultElement.Value;
    }
    public GenericItemScriptable GetItemByIndex(int index)
    {
        var resultElement = itemsDictionary.First(element => element.Key == index);
        return resultElement.Value;
    }
    public bool RemoveItemById(int id)
    {
        try
        {
            var resultItem = itemsDictionary.First(element => element.Value.Id == id);
            if (!(resultItem.Equals(null))){
                itemsDictionary.Remove(resultItem.Key);
                UpdateTotalWeight();
                return true;
            }
        }
        catch(System.Exception ex)
        {

        }

        return false;
    }
    private void UpdateTotalWeight()
    {
        currentWeightUse = 0;
        if (ItemsDictionary.Count != 0) foreach (var element in ItemsDictionary) currentWeightUse += element.Value.TotalWeightPerItem;
    }
    #endregion

}