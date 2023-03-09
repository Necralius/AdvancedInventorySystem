using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZoneBehaviorView : MonoBehaviour, IDropHandler
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //DropZoneBehaviorView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - On Drop Functionality -
    public void OnDrop(PointerEventData eventData)//This method handle the drop functionality using the unity Interface IDropHandler
    {
        RectTransform invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GameObject gameObjectResult = eventData.pointerDrag;
            GenericItemScriptable itemResult = gameObjectResult.GetComponent<ComplexSlotView>().ItemView;
            Vector2 coordinate = GetComponent<SimpleSlotView>().coordinate;

            SlotPlaceTo slotPlaceTo = GetComponent<SimpleSlotView>().slotPlaceTo;

            InventoryManagerController.Instance.OnDropItem(itemResult, gameObjectResult, coordinate, slotPlaceTo);//This statement execute the item drop functionality calling the InventoryManagerContoller static instance and calling the OnDropItem Method
        }
    }
    #endregion
}