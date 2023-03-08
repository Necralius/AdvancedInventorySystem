using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipActionScriptable", menuName = "InventorySystem/Action/NewEquipActionScriptable")]
public class EquipActionScriptable : GenericActionScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //EquipActionScriptable - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action Data -
    [SerializeField] private ItemEquip itemEquip;
    #endregion

    #region - Equip Action Execution -
    public override IEnumerator Execute()//This method represents the handable weapon or item equip execution
    {
        yield return new WaitForSeconds(DelayToStart);

        GameController.Instance.EquipChar(itemEquip);
    }
    #endregion
}