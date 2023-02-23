using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerController : MonoBehaviour
{
    #region - Main Declaration -
    [SerializeField] private List<GenericActionScriptable> currentActionList;

    #endregion

    #region - Methods -

    private void OnEnable()
    {
        ActionManagerEvent.SendActionListEvent += ReceiveActionList;
    }
    private void OnDisable()
    {
        ActionManagerEvent.SendActionListEvent -= ReceiveActionList;
    }
    private void ReceiveActionList(List<GenericActionScriptable> actionListReceived)
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
                Debug.LogError("The Action List is empty or currupted!");
            }
        }
        else Debug.LogWarning("There are any action inside current action List!");
    }
    #endregion
}