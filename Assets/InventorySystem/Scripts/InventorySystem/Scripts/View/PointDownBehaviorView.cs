using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointDownBehaviorView : MonoBehaviour, IPointerDownHandler
{
    private ComplexSlotView complexSlotView;

    #region - Methods -
    void Awake()
    {
        complexSlotView = GetComponent<ComplexSlotView>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GenericItemScriptable item = complexSlotView.ItemView;
        InventoryManagerController.Instance.OnPointerDownItem(item, this.gameObject);
    }
    #endregion
}