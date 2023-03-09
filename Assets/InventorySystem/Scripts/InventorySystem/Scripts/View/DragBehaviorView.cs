using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBehaviorView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //DragBehaviorView - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Data Declaration -
    private CanvasGroup canvasGroupCg => GetComponent<CanvasGroup>();
    private GameObject canvasIconGo;
    private GameObject iconGo;
    private Sprite iconImageSp;
    [SerializeField] private GameObject dragObjectParent;
    #endregion

    //=========== Method Area ===========//

    #region - Drag Behavior Calls -
    public void OnBeginDrag(PointerEventData eventData) => StartDrag();//This method maintain an public acess to the StartDrag method
    public void OnEndDrag(PointerEventData eventData) => StopDrag();//This method maintain an public acess to the StopDrag method
    public void OnDrag(PointerEventData eventData)//This method set the drag objet to the mouse position in real time
    {
        iconGo.transform.position = Input.mousePosition;
        iconGo.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }
    #endregion

    #region - Drag End Behavior -
    private void StopDrag() => Destroy(iconGo, 0.05f);//This method destroy the created object for the purpuso to maintain the project optimazed
    #endregion

    #region - On Drag Start SetUp -
    private void StartDrag()//This method SetUp all the necessary UI to the drag object
    {
        iconGo = new GameObject("Icon");
        iconGo.AddComponent<Image>();
        iconGo.AddComponent<CanvasGroup>();

        iconImageSp = GetComponent<ComplexSlotView>().ItemView.Icon;
        iconGo.GetComponent<Image>().sprite = iconImageSp;
        iconGo.GetComponent<Image>().raycastTarget = false;

        iconGo.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        iconGo.transform.SetParent(InventoryView.Instance.gameObject.transform);
        iconGo.GetComponent<CanvasGroup>().alpha = 0.65f;
    }
    #endregion
}