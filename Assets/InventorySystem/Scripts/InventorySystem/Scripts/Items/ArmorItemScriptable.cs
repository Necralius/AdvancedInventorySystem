using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorItem", menuName = "InventorySystem/Items/Armor Items")]
public class ArmorItemScriptable : GenericItemScriptable
{
    [SerializeField] private List<GenericActionScriptable> actionEquipList;

    public override void ActionEquipandUnequipListDispatch()
    {
        actionManagerEvnt = new ActionManagerEvent();
        actionManagerEvnt.DispatchAllGenericActionListEvent(actionEquipList);
    }
    public override ItemType GetItemType() => ItemType.ARMOR;
}