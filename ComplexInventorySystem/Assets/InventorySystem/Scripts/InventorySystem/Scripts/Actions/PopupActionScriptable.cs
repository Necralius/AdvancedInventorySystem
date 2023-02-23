using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPopupAction", menuName = "InventorySystem/Action/NewPopupAction")]
public class PopupActionScriptable : GenericActionScriptable
{
    [SerializeField, TextArea(5, 8)] private string description;

    [SerializeField] private Sprite icon;

    [SerializeField] private Color textcolor;
    [SerializeField] private Color backgroundColor;

    [SerializeField, Range(0, 7)] float timeToClosePopup;

    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(DelayToStart);

        //Game Controller -> ShowPopup

        /*
         
          
        */
    }


}