using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "InventorySystem/Items/Weapon Items")]
public class WeaponItemScriptable : GenericItemScriptable
{
    [SerializeField] private List<GenericActionScriptable> actionEquipList;

    public override void ActionEquipandUnequipListDispatch()
    {
        actionManagerEvnt = new ActionManagerEvent();
        actionManagerEvnt.DispatchAllGenericActionListEvent(actionEquipList);
    }

}
