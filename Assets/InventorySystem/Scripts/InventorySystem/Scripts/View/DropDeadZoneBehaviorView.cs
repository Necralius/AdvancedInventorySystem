using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropDeadZoneBehaviorView : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GameObject gameObjectResult = eventData.pointerDrag;
            GenericItemScriptable itemResult = gameObjectResult.GetComponent<ComplexSlotView>().ItemView;
            string originTag = LookForTag(gameObjectResult);

            InventoryManagerController.Instance.OnDropItem(itemResult, originTag);
        }
    }
    private string LookForTag(GameObject obj)
    {
        //Inventory
        if (obj.tag.Equals("ComplexSlot")) return "ComplexSlot";

        if (obj.transform.parent.tag.Equals("SpecialSlot")) return "SpecialSlot";

        if (obj.transform.parent.tag.Equals("ClothingWeaponsSlot")) return "ClothingWeaponsSlot";

        return "Untagged";
    }
}