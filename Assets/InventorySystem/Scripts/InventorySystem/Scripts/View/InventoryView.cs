using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InventoryView : MonoBehaviour//This class manage and control all the inventory system UI
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //InventoryView - Code Update Version 0.5 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement means a simple Singleton Pattern implementation
    public static InventoryView Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Main Data Declaration -
    [Header("View Declaration")]
    private GenericBagScriptable currentBag;
    private bool visiblePanel;

    [SerializeField] private string separationChar = "/";
    [SerializeField] private string weightUnit = " Kg";
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

    //=========== Method Area ===========//

    #region - Inventory View Inicitation -
    private void OnEnable() => visiblePanel = inventoryGo.activeSelf;
    public void Initiate(GenericBagScriptable currentBag)//This method iniciate all the inventory functionality, and update all the needed UI
    {
        this.currentBag = currentBag;
        SlotAndGridUpdate(this.currentBag.MaxRow, this.currentBag.MaxColumn);
        BagIconAndTitleupdate();
        BagWeightAndSlotUpdate();
    }
    #endregion

    #region - Slot Grid Update -
    private void SlotAndGridUpdate(int maxRow, int maxColumn)//This method generates the item slot grid based in the number of row an collumn passed in the arguments
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
    #endregion

    #region - Bag UI Update -
    private void BagIconAndTitleupdate()//This method updates the bag Title on the bag UI
    {
        bagIcon.sprite = currentBag.Icon;
        bagTittle.text = currentBag.Title;
    }
    public void BagWeightAndSlotUpdate()//This method updates the bag Slot Quantity and weight on the bag UI
    {
        float maxWeight = currentBag.WeightLimited;
        float maxSlot = currentBag.SlotLimited;

        string weight = currentBag.CurrentWeightUse.ToString() + separationChar + maxWeight.ToString() + weightUnit;
        bagWeight.text = weight;

        string slot = currentBag.CurrentSlot.ToString() + separationChar + maxSlot.ToString() + " Slot";
        bagSlot.text = slot;
    }
    #endregion

    #region - Complex Item Inventory Update -
    public void UpdateAllItems(List<GenericItemScriptable> list)//This method receive an item list as argument, then remove all complex slots and rebuild it
    {
        if (GameManager.Instance.inventoryIsOpen)
        {
            RemoveAllComplexSlot();
            foreach(var item in list) BuildComplexSlot(item);
            BagWeightAndSlotUpdate();
        }
    }
    #endregion

    #region - Complex Slot Building -
    private void BuildComplexSlot(GenericItemScriptable item)//This method build an item complex slot based on the item size
    {
        Vector3 pos = new Vector3(0, 0, 0);
        Vector2 size = new Vector2(cellSize, cellSize);

        Vector2 factor = new Vector2(1, 1);

        List<Vector2> cellList = currentBag.FindCellById(item.Id);//This statement search the item on the matrix space using the item id as an argument

        if (cellList.Count > 1)//The following statements calculates the complex slot size
        {
            factor = cellList[cellList.Count - 1] - cellList[0];

            size = new Vector2((cellSize * factor.y) + cellSize, (cellSize * factor.x) + cellSize);
            if (size.x == 0) size = new Vector2(cellSize, size.y);
            if (size.y == 0) size = new Vector2(size.x, cellSize);
        }

        pos = new Vector3(cellSize * cellList[0].y, (cellSize * cellList[0].x * -1));//This statement calculates the item complex slot position

        //The following statements instatiate and setup the complex slot, later also update all his UI 
        GameObject obj = Instantiate(complexSlotGo);

        obj.transform.SetParent(complexSlotGroup.transform);
        obj.GetComponent<RectTransform>().localPosition = pos;
        obj.GetComponent<RectTransform>().sizeDelta = size;
        obj.GetComponent<ComplexSlotView>().ItemView = item;
        obj.GetComponent<ComplexSlotView>().UpdateIcon();
        obj.GetComponent<ComplexSlotView>().UpdateText();
        obj.tag = "ComplexSlot";
        obj.name = item.name + "_Clone";
    }
    #endregion
    
    #region - ComplexSlotRemoval -
    private void RemoveAllComplexSlot()//This method remove all complex slots of the inventory
    {
        GameObject[] resultComplexSlotGo = GameObject.FindGameObjectsWithTag("ComplexSlot");
        foreach (var item in resultComplexSlotGo) Destroy(item);
    }
    #endregion
    
    #region - Item Description Update -
    public void UpdateDescriptionPanel(GenericItemScriptable item)//This method update all the item description panel
    {
        if (visiblePanel)
        {
            itemText.text = "Item: " + item.name;
            typeText.text = "Type: " + item.GetItemType();
            descriptionText.text = item.Description;

            itemIconDetailPanel.sprite = item.Icon;

            currentQuantityText.text = item.CurrentQuantity.ToString();
            maxQuantityText.text = separationChar + item.MaxQuantity.ToString();
            totalWeightPerItemNumberText.text = item.TotalWeightPerItem.ToString() + weightUnit;
        }
    }
    #endregion
}