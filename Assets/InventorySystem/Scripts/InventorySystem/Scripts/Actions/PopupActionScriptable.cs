using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPopupAction", menuName = "InventorySystem/Action/NewPopupAction")]
public class PopupActionScriptable : GenericActionScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //GenericActionScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action Data -
    [SerializeField, TextArea(5, 8)] private string description;

    [SerializeField] private Sprite icon;

    [SerializeField] private Color textcolor;
    [SerializeField] private Color backgroundColor;

    [SerializeField, Range(0, 7)] float timeToClosePopup;
    #endregion

    #region - Action Execution -
    public override IEnumerator Execute()//This method represents the popup action execution
    {
        yield return new WaitForSeconds(DelayToStart);

        GameController.Instance.ShowPopup(description, icon, textcolor, backgroundColor, timeToClosePopup);
    }
    #endregion
}