using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InventoryView : MonoBehaviour
{
    #region - Singleton Pattern -
    public static InventoryView Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Data Declaration -
    [Header("View Declaration")]
    private GenericBagScriptable currentBag;
    private bool visiblePanel;

    [SerializeField] private string separationChar = "/";
    [SerializeField] private string weightUnit = " Kg";
    private string n = " Slot";
    private float cellSize = 50;

    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private GameObject slotGroup;
    [SerializeField] private GameObject slotGo;
    [SerializeField] private GameObject complexSlotGroup;

    [SerializeField] public GameObject complexSlotGo;

    #endregion

    #region - TitlePanel -

    [Header("Title Panel")]
    [SerializeField] private Image bagIcon;
    [SerializeField] private TextMeshProUGUI bagTittle;
    [SerializeField] private TextMeshProUGUI bagWeight;
    [SerializeField] private TextMeshProUGUI bagSlot;

    [Header("Item Panel")]
    [SerializeField] private Image itemIconTitlePanel;
    [SerializeField] private TextMeshProUGUI itemCurrentNumber;
    [SerializeField] private TextMeshProUGUI itemMaxNumber;
    [SerializeField] private TextMeshProUGUI itemCurrentWeight;
    [SerializeField] private TextMeshProUGUI itemMaxWeight;
    #endregion

    #region - Description Panel -
    [Header("Description Panel")]
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    #endregion

    #region - Detail Panel -
    [Header("Detail Panel")]
    [SerializeField] private Image itemIconDetailPanel;
    [SerializeField] TextMeshProUGUI currentQuantityText;
    [SerializeField] TextMeshProUGUI maxQuantityText;
    [SerializeField] TextMeshProUGUI totalWeightPerItemNumberText;
    #endregion

    #region - Designer and Slot Control -
    [Header("Designer and Slot Control")]
    [SerializeField] private List<GameObject> simpleSlotList;
    [SerializeField] private GridLayoutGroup gridController;

    #endregion

    #region - Getting Data -
    public bool VisiblePanel { get => visiblePanel; }
    #endregion

    #region - Methods -
    private void OnEnable()
    {
        visiblePanel = inventoryGo.activeSelf;
    }

    public void Initiate(GenericBagScriptable currentBag)
    {
        this.currentBag = currentBag;
        SlotAndGridUpdate(this.currentBag.MaxRow, this.currentBag.MaxColumn);
        BagIconAndTitleupdate();
        BagWeightAndSlotUpdate();
    }

    private void SlotAndGridUpdate(int maxRow, int maxColumn)
    {
        int r = 0;
        int c = 0;

        if (maxColumn <= 9)
        {
            int ajust = 500 - (maxColumn * 50);
            gridController.padding.right = ajust;
        }

        for (int i = 0; i < maxRow * maxColumn; i++)
        {
            GameObject currentSlotGo = Instantiate(slotGo);
            currentSlotGo.GetComponent<SimpleSlotView>().coordinate = new Vector2(r, c);
            currentSlotGo.tag = "SlotSimple";
            c++;
            if(c == maxColumn)
            {
                c = 0;
                r++;
            }
            currentSlotGo.transform.SetParent(slotGroup.transform);
        }
    }
    private void BagIconAndTitleupdate()
    {
        bagIcon.sprite = currentBag.Icon;
        bagTittle.text = currentBag.Title;
    }
    public void BagWeightAndSlotUpdate()
    {
        float maxWeight = currentBag.WeightLimited;
        float maxSlot = currentBag.SlotLimited;

        string weight = currentBag.CurrentWeightUse.ToString() + separationChar + maxWeight.ToString() + weightUnit;
        bagWeight.text = weight;

        string slot = currentBag.CurrentSlot.ToString() + separationChar + maxSlot.ToString() + " Slot";
        bagSlot.text = slot;
    }
    public void UpdateAllItems(List<GenericItemScriptable> list)
    {
        //Check Inventory Panel
        BagWeightAndSlotUpdate();

    }

    #endregion
}