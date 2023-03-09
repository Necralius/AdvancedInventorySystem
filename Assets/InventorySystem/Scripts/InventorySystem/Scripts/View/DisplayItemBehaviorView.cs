using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayItemBehaviorView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //DisplayItemBehaviorView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Data Declaration -
    [SerializeField] private GameObject simpleSlotGo;
    [SerializeField] private GameObject complexSlotGo;
    #endregion

    #region - Item Display Behavior -
    private void OnEnable() => TurnOff();//This method deactivate the slot on the object instatiation
    public void TurnOn() => complexSlotGo.SetActive(true);//This method deactivate the item ComplexSlot that show the item UI representation
    public void TurnOff() => complexSlotGo.SetActive(false);//This method activate the item ComplexSlot that show the item UI representation
    #endregion
}