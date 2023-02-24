using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBehaviorView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroupCg;
    private GameObject canvasIconGo;
    private GameObject iconGo;
    private Sprite iconImageSp;

    #region - Data Declaration -

    #endregion

    #region - Methods -

    private void Awake()
    {
        canvasGroupCg = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        iconGo.transform.position = Input.mousePosition;
        iconGo.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StopDrag();
    }
    private void StartDrag()
    {
        iconGo = new GameObject("Icon");
        iconGo.AddComponent<Image>();
        iconGo.AddComponent<CanvasGroup>();

        iconImageSp = GetComponent<ComplexSlotView>().ItemView.Icon;
        iconGo.GetComponent<Image>().sprite = iconImageSp;
        iconGo.GetComponent<Image>().raycastTarget = false;

        iconGo.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        iconGo.transform.SetParent(gameObject.transform);
        iconGo.GetComponent<CanvasGroup>().alpha = 0.65f;
    }
    private void StopDrag()
    {
        Destroy(iconGo, 0.05f);
    }
    #endregion
}