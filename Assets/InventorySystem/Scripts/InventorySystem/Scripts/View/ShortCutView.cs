using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShortCutView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static ShortCutView Instance;
    public void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -

    [SerializeField] private GameObject shortCutGO;
    [SerializeField] private GameObject slotSpecialGroup;
    [SerializeField] private GameObject slotSpecialSlotGo;

    private Dictionary<int, GameObject> specialSlotDictionary;

    #endregion

    #region - Methods -
    public void IniciateShortCutSlots(int maxShortSlot)
    {
        for (int column = 0; column < maxShortSlot; column++)
        {
            GameObject resultGo = Instantiate(slotSpecialSlotGo);
            SimpleSlotView[] resultSimpleSlotView = resultGo.GetComponentsInChildren<SimpleSlotView>();

            if (!(resultSimpleSlotView[0].Equals(null)))
            {
                resultSimpleSlotView[0].coordinate = new Vector2 (0, column);
            }
            resultGo.transform.SetParent(slotSpecialGroup.transform);
        }
        specialSlotDictionary = new Dictionary<int, GameObject>();
        GameObject[] resultList = GameObject.FindGameObjectsWithTag("SpecialSlot");
        int index = 0;

        foreach(var item in resultList)
        {
            specialSlotDictionary.Add(index, item);
            index++;
        }
    }
    public void RefreshSlotSystem(Dictionary<int, GenericItemScriptable> itemsShortCutDictionary)
    {
        foreach(var item in specialSlotDictionary)
        {
            item.Value.GetComponent<DisplayItemBehaviorView>().TurnOff();
        }
        if (!(itemsShortCutDictionary.Equals(null)))
        {
            foreach(var item in itemsShortCutDictionary)
            {
                GameObject resultGo = SelectedSpecialSlotByIndex(item.Key);
                resultGo.GetComponent<DisplayItemBehaviorView>().TurnOn();
                resultGo.GetComponentInChildren<ComplexSlotView>().ItemView = item.Value;
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateText();
                resultGo.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
            }
        }
    }
    public void UpdateSlot(Dictionary<int, GenericItemScriptable> itemsShortCutDictionary, List<int> usedKeys)
    {
        foreach (var key in usedKeys)
        {
            var resultSpecialSlotDictionary = specialSlotDictionary.First(element => element.Key == key);
            resultSpecialSlotDictionary.Value.GetComponent<DisplayItemBehaviorView>().TurnOn();

            var resultItemShortCutDictionary = itemsShortCutDictionary.First(element => element.Key == key);
            GenericItemScriptable resultItem = resultItemShortCutDictionary.Value;

            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().ItemView = resultItem;
            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().UpdateText();
            resultSpecialSlotDictionary.Value.GetComponentInChildren<ComplexSlotView>().UpdateIcon();
        }
    }
    private GameObject SelectedSpecialSlotByIndex(int id)
    {
        try
        {
            var result = specialSlotDictionary.First(element => element.Key == id);

            if (!(result.Equals(null))) return result.Value;
        }
        catch(System.Exception ex)
        {
            Debug.LogException(ex);
        }
        return null;
    }
    #endregion
}