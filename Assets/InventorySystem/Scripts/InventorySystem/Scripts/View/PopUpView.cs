using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //PopUpView - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static PopUpView Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Main Data Declaration -
    [SerializeField] private GameObject popupGo;
    private float delayToClose = 0.05f;
    #endregion

    //=========== Method Area ===========//

    #region - Popup Setup and Action -
    private void Start() => popupGo.SetActive(false);//This method deactivate the popup UI on the scene start
    public void PopupUpdate(string message, Sprite icon, Color textColor, Color backgroundColor, float timeToClosePopup)//This method setup the popup UI and activate his functionality
    {
        popupGo.SetActive(true);
        popupGo.GetComponentInChildren<TextMeshProUGUI>().text = message;
        popupGo.GetComponentInChildren<Image>().sprite = icon;
        popupGo.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        popupGo.GetComponent<Image>().color = backgroundColor;

        if (timeToClosePopup != 0)
        {
            delayToClose = timeToClosePopup;
            StartCoroutine("Close");
        }
    }
    IEnumerator Close()//This method deactivate the popup UI when the action time has elapsed
    {
        yield return new WaitForSeconds(delayToClose);
        popupGo.SetActive(false);
    }
    #endregion
}