using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropDeadZoneBehaviorView : MonoBehaviour, IDropHandler
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //DropDeadZoneBehaviorView - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Item Drop Behavior - 
    public void OnDrop(PointerEventData eventData)//This method handle the drop on the dead zone functionality using the unity Interface IDropHandler
    {
        RectTransform invPanel = transform as RectTransform;

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GenericItemScriptable itemResult;
            GameObject gameObjectResult = eventData.pointerDrag;
            if (gameObjectResult.GetComponent<ComplexSlotView>().ItemView) itemResult = gameObjectResult.GetComponent<ComplexSlotView>().ItemView;
            else itemResult = null;

            string originTag = LookForTag(gameObjectResult);

            InventoryManagerController.Instance.OnDropItem(itemResult, originTag);
        }
    }
    private string LookForTag(GameObject obj)//This method returns an object tag based on an GameObject passed in the arguments
    {
        if (obj.tag.Equals("ComplexSlot")) return "ComplexSlot";
        if (obj.transform.parent.tag.Equals("SpecialSlot")) return "SpecialSlot";
        if (obj.transform.parent.tag.Equals("ClothingWeaponsSlot")) return "ClothingWeaponsSlot";

        return "Untagged";
    }
    #endregion
}