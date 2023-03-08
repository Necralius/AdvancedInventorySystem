using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "InventorySystem/Items/Weapon Items")]
public class WeaponItemScriptable : GenericItemScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //WeaponItemScriptable - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Weapon Equip Action -
    [SerializeField] private List<GenericActionScriptable> actionEquipList;
    #endregion

    #region - Weapon Equip Action Dispatch -
    public override void ActionEquipandUnequipListDispatch()//This method pass the weapon equip action list to the ActionManagerEvent classe
    {
        actionManagerEvnt = new ActionManagerEvent();
        actionManagerEvnt.DispatchAllGenericActionListEvent(actionEquipList);
    }
    public override ItemType GetItemType() => ItemType.WEAPON;//This method return the ItemType
    #endregion
}