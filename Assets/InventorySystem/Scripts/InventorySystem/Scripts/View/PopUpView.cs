using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static PopUpView Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -
    [SerializeField] private GameObject popupGo;

    private float delayToClose = 0.05f;

    #endregion

    #region - Methods -

    private void Start() => popupGo.SetActive(false);

    public void PopupUpdate(string message, Sprite icon, Color textColor, Color backgroundColor, float timeToClosePopup)
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
    IEnumerator Close()
    {
        yield return new WaitForSeconds(delayToClose);
        popupGo.SetActive(false);
    }

    #endregion



}