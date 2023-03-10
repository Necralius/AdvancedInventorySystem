using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointDownBehaviorView : MonoBehaviour, IPointerDownHandler
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //PointDownBehaviorView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Main Data Declaration -
    private ComplexSlotView complexSlotView;
    #endregion

    #region - Point Down Action -
    void Awake() => complexSlotView = GetComponent<ComplexSlotView>();
    public void OnPointerDown(PointerEventData eventData)//This method execute all the UI update on the details window
    {
        GenericItemScriptable item = complexSlotView.ItemView;
        InventoryManagerController.Instance.OnPointerDownItem(item, this.gameObject);
    }
    #endregion
}