using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName = "InventorySystem/Items/Consumable Items")]
public class ConsumableItemScriptable : GenericItemScriptable
{
    public override ItemType GetItemType() => ItemType.CONSUMABLE;
}