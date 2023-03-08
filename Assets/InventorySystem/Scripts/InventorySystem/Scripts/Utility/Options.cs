using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options { }
//Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
//EnumStorage - Code Update Version 0.4 - (Refactored code).
//Feel free to take all the code logic and apply in yours projects.
//This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

#region - Slot Indentification
public enum SlotPlaceTo //This enumerator represents all the type of slot indentification
{
    BAG = 0,
    CLOTHING_WEAPON = 1,
    SHORT_CUT = 2,
    OUT = 3,
}
#endregion

#region - Item Types -
public enum ItemType //This enumerator represents all the items possible types
{
    CONSUMABLE = 0,
    STORY = 1,
    WEAPON = 2,
    ARMOR = 3
}
#endregion

#region - Item Equip Types -
public enum ItemEquip //This enumerator represents all the item equip possible types
{
    NONE = 0,
    ARMOR_KEVLAR = 1,
    ARMOR_BALISTIC_HELMET = 2,
    Handable_FireGun_AK47 = 3,
    Handable_FireGun_M4A4_Carbine = 4,
    Handable_FireGun_M9 = 5,
    Handable_FireGun_M24 = 6,
    Handable_FireGun_Glock17 = 7,
    Handable_FireGun_UMP45 = 8,
}
#endregion