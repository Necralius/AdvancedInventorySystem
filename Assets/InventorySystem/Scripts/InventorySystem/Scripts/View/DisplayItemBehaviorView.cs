using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItemBehaviorView : MonoBehaviour
{
    #region - Data Declaration -
    [SerializeField] private GameObject simpleSlotGo;
    [SerializeField] private GameObject complexSlotGo;
    #endregion

    #region - Methods 
    private void OnEnable() => TurnOff();
    public void TurnOn() => complexSlotGo.SetActive(true);
    public void TurnOff() => complexSlotGo.SetActive(false);
    #endregion
}