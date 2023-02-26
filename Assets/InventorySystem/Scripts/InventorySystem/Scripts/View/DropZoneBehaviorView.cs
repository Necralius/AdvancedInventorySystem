using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZoneBehaviorView : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            GameObject gameObjectResult = eventData.pointerDrag;
            GenericItemScriptable itemResult = gameObjectResult.GetComponent<ComplexSlotView>().ItemView;
            Vector2 coordinate = GetComponent<SimpleSlotView>().coordinate;

            SlotPlaceTo slotPlaceTo = GetComponent<SimpleSlotView>().slotPlaceTo;

            bool result = InventoryManagerController.Instance.OnDropItem(itemResult, gameObjectResult, coordinate, slotPlaceTo);

            if (result)
            {
                Debug.Log("Item accepted!");
            }
        }
    }
}