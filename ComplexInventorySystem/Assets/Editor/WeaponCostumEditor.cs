using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSystem))]
public class WeaponCostumEditor : Editor
{
    WeaponSystem targetWeapon;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        targetWeapon = (WeaponSystem)target;

        if (targetWeapon.gunType == GunType.Shotgun)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullets Per Tap", GUILayout.MaxWidth(100f));
            targetWeapon.shootsPerTap = (int)EditorGUILayout.IntField(targetWeapon.shootsPerTap, GUILayout.MaxWidth(32f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Bullet Spread", GUILayout.MaxWidth(100f));
            targetWeapon.bulletSpread = (float)EditorGUILayout.FloatField(targetWeapon.bulletSpread, GUILayout.MaxWidth(32f));
            GUILayout.EndHorizontal();
        }
        else
        {
            targetWeapon.shootsPerTap = 1;
            targetWeapon.bulletSpread = 0;
        }
    }
}