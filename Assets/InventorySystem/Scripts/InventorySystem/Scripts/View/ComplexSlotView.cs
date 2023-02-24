using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComplexSlotView : MonoBehaviour
{
    #region - Data Declaration -
    [SerializeField] private GenericItemScriptable itemView;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI currenNumberText;
    private CanvasGroup canvasGroup;
    #endregion

    #region - Getting and Setting Data -
    public GenericItemScriptable ItemView { get => itemView; set => itemView = value; }
    #endregion

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void UpdateText()
    {
        currenNumberText.text = ItemView.CurrentQuantity.ToString();
        EnableAndDisableIcon(itemView.CurrentQuantity);
    }
    public void UpdateIcon()
    {
        icon.sprite = itemView.Icon;
    }
    private void EnableAndDisableIcon(int value)
    {
        if (value > 0) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0.3f;
    }
}