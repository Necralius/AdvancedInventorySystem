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
    private GenericBagScriptable currentBag;
    private bool visiblePanel;

    private string s = "/";
    private string w = " Kg";
    private string n = " Slot";
    private float cellSize = 50;

    [SerializeField] private GameObject canvasIcon;
    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private GameObject slotGroup;
    [SerializeField] private GameObject slotGo;
    [SerializeField] private GameObject complexSlotGroup;

    [SerializeField] public GameObject complexSlotGo;

    #endregion

    #region - TitlePanel -

    [SerializeField] private Image bagIcon;
    [SerializeField] private TextMeshProUGUI bagTittle;
    [SerializeField] private TextMeshProUGUI bagWeight;
    [SerializeField] private TextMeshProUGUI bagSlot;
    [SerializeField] private Image itemIconTitlePanel;
    [SerializeField] private TextMeshProUGUI itemCurrentNumber;
    [SerializeField] private TextMeshProUGUI itemMaxNumber;
    [SerializeField] private TextMeshProUGUI itemCurrentWeight;
    [SerializeField] private TextMeshProUGUI itemMaxWeight;


    #endregion

    #region - Description Panel -
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    #endregion

    #region - Detail Panel -
    [SerializeField] private Image itemIconDetailPanel;
    [SerializeField] TextMeshProUGUI currentQuantityText;
    [SerializeField] TextMeshProUGUI maxQuantityText;
    [SerializeField] TextMeshProUGUI totalWeightPerItemNumberText;

    #endregion

    #region - Designer and Slot Control -
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

    #endregion





}