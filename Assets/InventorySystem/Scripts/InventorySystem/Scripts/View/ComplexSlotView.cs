using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComplexSlotView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //ComplexSlotView - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Data Declaration -
    [SerializeField] private GenericItemScriptable itemView;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI currenNumberText;
    private CanvasGroup canvasGroup => GetComponent<CanvasGroup>();
    #endregion

    #region - Getting and Setting Data -
    public GenericItemScriptable ItemView { get => itemView; set => itemView = value; }
    #endregion

    #region - Complex Slot UI Update -
    public void UpdateText()//This method update the ComplexSlot UI quantity text and updates the item alpha color
    {
        currenNumberText.text = ItemView.CurrentQuantity.ToString();
        EnableAndDisableIcon(itemView.CurrentQuantity);
    }
    public void UpdateIcon() => icon.sprite = itemView.Icon;//This method updates the ComplexSlot image sprite
    private void EnableAndDisableIcon(int value)//This method enable and disable the item alpha color based on the value argument
    {
        if (value > 0) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0.3f;
    }
    #endregion
}