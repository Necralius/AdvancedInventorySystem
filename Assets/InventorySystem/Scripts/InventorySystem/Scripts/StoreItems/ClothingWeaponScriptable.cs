using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clothing Weapon", menuName = "InventorySystem/Store Items/New Clothing Weapon")]
public class ClothingWeaponScriptable : ScriptableObject
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ClothingWeaponScriptable - Code Update Version 0.5 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Data Declaration
    [SerializeField, Range(1, 8)] private int slotNumber;

    [SerializeField] private List<RulerScriptable> ruleList;

    [SerializeField] private Dictionary<int, GenericItemScriptable> itemsDictionary;

    [SerializeField] private float currentWeightUse;
    #endregion

    #region - Data Get and Set -
    //This statements protect and manage the item data setting and get
    public float CurrentWeightUse { get => currentWeightUse; }
    public Dictionary<int, GenericItemScriptable> ItemsDictionary { get => itemsDictionary; }
    public int SlotNumber { get => slotNumber; }
    #endregion

    //=========== Method Area ===========//

    #region - Item Start -
    private void OnEnable()//This method start the ClothingWeapon declaration
    {
        itemsDictionary = new Dictionary<int, GenericItemScriptable>();
        currentWeightUse = 0;
    }
    #endregion

    #region - Item Add -
    public bool AddItem(int index, GenericItemScriptable item)//This method adds an item to the clothing weapon slot
    {
        if (itemsDictionary.ContainsValue(item))//This statement return if the item its already in the slot
        {
            Debug.LogWarning("The item: " + item.name + "Cannot be added, its already in the slot!");
            return false;
        }
        else
        {
            if (CheckAllRules(index, item))//This statement check all the rules and returns if the item can or cannot be added
            {
                ItemsDictionary.Add(index, item);
                UpdateTotalWeight();
                return true;
            }
        }
        return false;
    }
    #endregion

    #region - Slots Rules Check -
    private bool CheckAllRules(int index, GenericItemScriptable item)//This method receive an item as argument, and check all the rules to see if the item can or cannot be added
    {
        //This method also uses the Try Catch block to to avoid total code break
        try
        {
            if (ruleList.Count != 0) foreach (var rule in ruleList) if (rule.Validade(index, item)) return true;
        }
        catch(System.Exception ex)
        {
            Debug.LogWarning("Do not Exists Rules in the list!");
        }
        return false;
    }
    #endregion

    #region - Item Gathering -
    public GenericItemScriptable GetItemById(int id)//This method search an item in the slots using the item id, then he return this item
    {
        var resultElement = itemsDictionary.First(element => element.Value.Id == id);
        return resultElement.Value;
    }
    public GenericItemScriptable GetItemByIndex(int index)//This method search an item in the slots using the slot index, then he return this item
    {
        var resultElement = itemsDictionary.First(element => element.Key == index);
        return resultElement.Value;
    }
    #endregion

    #region - Item Remove -
    public bool RemoveItemById(int id)//This method remove the item using the id as an indentificator 
    {
        //This method also uses the Try Catch block to to avoid total code break
        try
        {
            var resultItem = itemsDictionary.First(element => element.Value.Id == id);//This statement return the finded item in the shortcut dictionary
            if (!(resultItem.Equals(null))){
                itemsDictionary.Remove(resultItem.Key);
                UpdateTotalWeight();
                GameController.Instance.DequipAllWeapons();
                return true;
            }
        }
        catch(System.Exception ex)
        {

        }

        return false;
    }
    #endregion

    #region - Item Weight Update -
    private void UpdateTotalWeight()//This method update the total items weight 
    {
        currentWeightUse = 0;
        if (ItemsDictionary.Count != 0) foreach (var element in ItemsDictionary) currentWeightUse += element.Value.TotalWeightPerItem;
    }
    #endregion
}