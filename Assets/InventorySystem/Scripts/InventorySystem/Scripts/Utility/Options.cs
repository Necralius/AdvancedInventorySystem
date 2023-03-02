using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options { }

public enum SlotPlaceTo
{
    BAG = 0,
    CLOTHING_WEAPON = 1,
    SHORT_CUT = 2,
    OUT = 3,
}
public enum ItemType
{
    CONSUMABLE = 0,
    STORY = 1,
    WEAPON = 2,
    ARMOR = 3
}
public enum ItemEquip
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
public enum SliderType
{
    Health = 0,
    Hungry = 1,
    Thirsty = 2,
    Stamina = 3,
    ColdLevel = 4,
}
public enum AttackType
{
    GUNPLAY = 0,
}