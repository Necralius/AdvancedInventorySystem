using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "InventorySystem/Items/Consumable Items")]
public class ConsumableItemScriptable : GenericItemScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ConsumableItemScriptable - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    public override ItemType GetItemType() => ItemType.CONSUMABLE;//This method return the ItemType
}