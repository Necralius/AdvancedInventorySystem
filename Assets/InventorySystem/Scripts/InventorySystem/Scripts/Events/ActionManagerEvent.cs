using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerEvent : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //Action Manager Event - Code Update Version 0.1 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - ActionManager Data -
    public delegate void SendActionList(List<GenericActionScriptable> actionList);
    public static event SendActionList SendActionListEvent;
    #endregion

    #region - Action Dispatch -
    public void DispatchAllGenericActionListEvent(List<GenericActionScriptable> actionList)//This method send all the actions from the list
    {
        if (SendActionListEvent != null) SendActionListEvent(actionList);
    }
    #endregion
}