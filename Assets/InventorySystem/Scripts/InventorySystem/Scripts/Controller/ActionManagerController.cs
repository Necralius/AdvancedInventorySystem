using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerController : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //Action Manager Controller - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action List -
    [SerializeField] private List<GenericActionScriptable> currentActionList;
    #endregion

    #region - Action Management Methods -
    private void OnEnable() => ActionManagerEvent.SendActionListEvent += ReceiveActionList;
    private void OnDisable() => ActionManagerEvent.SendActionListEvent -= ReceiveActionList;
    private void ReceiveActionList(List<GenericActionScriptable> actionListReceived)//This method uses an try catch block to get all the current actions on list and execute one by one, when executed, the action is removed of the list, thus making a simple queue
    {
        if (actionListReceived.Count > 0) 
        {
            currentActionList.AddRange(actionListReceived);

            try
            {
                while(currentActionList.Count > 0)
                {
                    StopCoroutine(currentActionList[0].Execute());
                    StartCoroutine(currentActionList[0].Execute());
                    currentActionList.Remove(currentActionList[0]);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("The Action List is currupted!");
            }
        }
        else Debug.LogWarning("There are any action inside current action List!");
    }
    #endregion
}