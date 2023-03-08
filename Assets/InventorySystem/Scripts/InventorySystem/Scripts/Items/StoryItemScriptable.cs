using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryItem", menuName = "InventorySystem/Items/Story Items")]
public class StoryItemScriptable : GenericItemScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //StoryItemScriptable - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    public override ItemType GetItemType() => ItemType.STORY;//This method return the ItemType
}