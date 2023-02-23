using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipActionScriptable", menuName = "InventorySystem/Action/NewEquipActionScriptable")]
public class EquipActionScriptable : GenericActionScriptable
{
    [SerializeField] private ItemEquip itemEquip;

    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(DelayToStart);

        //Game Controller -> EquipChar()
    }
}
