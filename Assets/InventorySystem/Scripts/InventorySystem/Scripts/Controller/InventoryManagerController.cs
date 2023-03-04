using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManagerController : MonoBehaviour
{
    #region - Singleton Pattern -
    public static InventoryManagerController Instance;
    void Awake()
    {
        Instance = this;
        ClothingWeaponView.Instance.Iniciate(currentClothingWeapon);
    }
    #endregion

    #region - Data Declaration - 
    [SerializeField] private GameObject playerObject;
    [SerializeField] GameObject directionToLaunchItem;
    [SerializeField] GameObject placeToDrop;

    [SerializeField] List<GameObject> itemList;

    [SerializeField] GenericBagScriptable currentBag;
    [SerializeField] private ClothingWeaponScriptable currentClothingWeapon;
    #endregion

    [SerializeField] protected List<KeyCode> keyCodeShortcutList;

    #region - Methods -
    private void Start()
    {
        InventoryView.Instance.Initiate(currentBag);
        BagScriptable resultBag = CastGenericBagToBag();
        ShortCutView.Instance.IniciateShortCutSlots(resultBag.MaxShortCutSlot);
    }
    private void Update()
    {
        if (Input.GetKeyDown(keyCodeShortcutList[0])) UseShortCutToUseItem(0);
        if (Input.GetKeyDown(keyCodeShortcutList[1])) UseShortCutToUseItem(1);
        if (Input.GetKeyDown(keyCodeShortcutList[2])) UseShortCutToUseItem(2);
        if (Input.GetKeyDown(keyCodeShortcutList[3])) UseShortCutToUseItem(3);
        if (Input.GetKeyDown(keyCodeShortcutList[4])) UseShortCutToUseItem(4);
        if (Input.GetKeyDown(keyCodeShortcutList[5])) UseShortCutToUseItem(5);
        if (Input.GetKeyDown(keyCodeShortcutList[6])) UseShortCutToUseItem(6);
        if (Input.GetKeyDown(keyCodeShortcutList[7])) UseShortCutToUseItem(7);
        if (Input.GetKeyDown(keyCodeShortcutList[8])) UseShortCutToUseItem(8);
        if (Input.GetKeyDown(keyCodeShortcutList[9])) UseShortCutToUseItem(9);
    }
    public bool AddItemToCurrentBag(GenericItemScriptable newItem, int quantity, bool exceptionCase)
    {
        bool result = false;

        if (!GameManager.Instance.inventoryIsOpen || exceptionCase)//Exception Case Verification
        {
            result = currentBag.AddItem(newItem, quantity);

            if (result)
            {
                List<GenericItemScriptable> resultItemList = currentBag.ReturnFullList();
                InventoryView.Instance.UpdateAllItems(resultItemList);
            }
            if (currentBag.UsedOrganizeBtSizePriority)
            {
                List<GenericItemScriptable> resultItemList = currentBag.ReturnFullList();

                InventoryView.Instance.UpdateAllItems(resultItemList);
                currentBag.UsedOrganizeBtSizePriority = false;
            }
            RefreshShortCutByItem(newItem);
        }
        return result;
    }
    private void UseItem(int id, int quantity)
    {
        bool result = currentBag.UseItem(id, quantity);

        if (result)
        {
            GenericItemScriptable itemResult = currentBag.FindItemById(id);
            RefreshShortCutByItem(itemResult);

            if (itemResult.CurrentQuantity == 0 && itemResult.RemoveWhenNumberIsZero) DropItem(itemResult);
        }
    }
    public void OrganizeItem()//This method organize the item UI representation based in his size
    {
        currentBag.OrgnizeBySizePriority();
        List<GenericItemScriptable> resultList = currentBag.ReturnFullList();
        InventoryView.Instance.UpdateAllItems(resultList);
        StartCoroutine("RefreshShortCutView");
        currentBag.UsedOrganizeBtSizePriority = false;
    }
    public bool RemoveItem(GenericBagScriptable origin, int id, int index)
    {
        bool result = false;

        if (origin != null && id >= 0)
        {
            result = origin.RemoveItem(id);
            RemoveItemFromShortCut(id);
            StartCoroutine("RefreshInventoryView");//This statment call the inventory UI refresh method
        }
        return true;
    }
    public bool DropItem(GenericItemScriptable item)
    {
        bool result = false;
        int currentQuantity;

        if (item.Id >= 0)//This prevent the negative items ID
        {
            currentQuantity = item.CurrentQuantity;
            result = currentBag.DropItem(item.Id);

            if (result)
            {
                StartCoroutine("RefreshInventoryView");//This statment call the inventory UI refresh method

                BagScriptable resultBag = CastGenericBagToBag();

                List<int> idList = resultBag.GetIdsFromItemShortCutDictionary();

                foreach (var idShortCut in idList) if (idShortCut == item.Id) RemoveItemFromShortCut(item.Id);//This remove item from his shortcut (if he is on a shortcut)

                BuildMeshModel(item.Id, currentQuantity);//This call the item drop GameObject instatiation
                return true;
            }
        }
        return false;
    }

    private void BuildMeshModel(int id, int currentQuantity)
    {
        //This method instatiate the GameObject of the item drop and add an physic force to throw it away
        GameObject itemResult = itemList.Find(item => item.GetComponent<ItemView>().Item.Id == id);

        if (itemResult != null)
        {
            Quaternion playerRotation = playerObject.transform.rotation;
            Vector3 spawnPos = directionToLaunchItem.transform.forward + directionToLaunchItem.transform.position;

            GameObject obj = Instantiate(itemResult, spawnPos, playerRotation);

            obj.name = obj.GetComponent<ItemView>().Item.name + "_" + Time.realtimeSinceStartup;
            obj.GetComponent<ItemView>().Quantity = currentQuantity;
            obj.transform.SetParent(placeToDrop.transform, true);
            obj.GetComponentInChildren<Rigidbody>().AddRelativeForce(0, Random.Range(50, 250), Random.Range(75, 300));
        }
        else Debug.LogWarning("There is no item with this id in the item list to instantiate: ItemList: " + itemList.ToString() + "Item ID: " + id);
    }
    private IEnumerator RefreshInventoryView()//This method refresh all the inventory UI
    {
        yield return new WaitForSeconds(0.02f);
        List<GenericItemScriptable> resultList = currentBag.ReturnFullList();
        InventoryView.Instance.UpdateAllItems(resultList);
    }
    #endregion

    #region - OnDropItem and OnPointDownItem -
    public bool OnDropItem(GenericItemScriptable itemDrop, GameObject origin, Vector2 coordinate, SlotPlaceTo slotPlaceTo)
    {
        //This statments represent the item drop functionality
        int index = (int)coordinate.y;
        if (origin.transform.parent.parent.name == "img_GridBackground" && slotPlaceTo != SlotPlaceTo.BAG)
        {
            if (slotPlaceTo == SlotPlaceTo.SHORT_CUT) return AddItemToCurrentShortCut(index, itemDrop);//This statment execute the item add to the shortcut Slot
            if (slotPlaceTo == SlotPlaceTo.CLOTHING_WEAPON) return TransferItemFromBagToClothingWeapon(index, itemDrop.Id);//This statment transfer the item from bag to the Clothing/Weapon Slot
        }
        if (origin.transform.parent.parent.name == "cont_ShortCutSlotsGroup" && slotPlaceTo == SlotPlaceTo.SHORT_CUT) return ChangeShortCutPosition(itemDrop, index);//This statment transfer the item betwen shortcut slots
        return false;
    }
    public bool OnDropItem(GenericItemScriptable itemDrop, string originTag)
    {
        //This statments represent the item drop limitation
        if (originTag.Equals("ComplexSlot"))
        {
            currentClothingWeapon.RemoveItemById(itemDrop.Id);
            StartCoroutine("RefreshClothingWeaponView");
            return DropItem(itemDrop);
        }
        if (originTag.Equals("SpecialSlot")) return RemoveItemFromShortCut(itemDrop.Id);
        //if (originTag.Equals("ClothingWeaponsSlot")) return TransferItemFromClothingWeaponToBag(itemDrop.Id);

        return false;
    }
    public void OnPointerDownItem(GenericItemScriptable item, GameObject origin)
    {
        if (origin.transform.parent.parent.name == ("img_GridBackground")) InventoryView.Instance.UpdateDescriptionPanel(item);
    }
    #endregion

    #region - Clothing Weapon Methods -

    public bool TransferItemFromBagToClothingWeapon(int index, int id)
    {
        GenericItemScriptable item = currentBag.FindItemById(id);

        AddItemToCurrentShortCut(index, item);
        bool result = currentClothingWeapon.AddItem(index, item);
        if (result)
        {
            StartCoroutine("RefreshClothingWeaponView");
            item.ActionEquipandUnequipListDispatch();
            return AddItemToCurrentShortCut(index, item);

            //return RemoveItem(currentBag, id, index);
        }
        return false;
    }
    public bool TransferItemFromClothingWeaponToBag(int id)
    {
        GenericItemScriptable item = currentClothingWeapon.GetItemById(id);

        bool result = currentClothingWeapon.RemoveItemById(id);
        if (result)
        {
            item.ActionEquipandUnequipListDispatch();
            StartCoroutine("RefreshClothingWeaponView");
            RemoveItemFromShortCut(id);
            return AddItemToCurrentBag(item, 0, true);
        }
        return false;
    }
    public void CallRefreshClothingWeaponView() => StartCoroutine("RefreshClothingWeaponView");
    private IEnumerator RefreshClothingWeaponView()
    {
        yield return new WaitForSeconds(0.01f);
        Dictionary<int, GenericItemScriptable> resultDictionary = currentClothingWeapon.ItemsDictionary;
        ClothingWeaponView.Instance.RefreshSlotSystem(resultDictionary);
    }
    #endregion

    #region - ShortCut Methods -
    private bool AddItemToCurrentShortCut(int index, GenericItemScriptable item)
    {
        BagScriptable resultBag = CastGenericBagToBag();
        if (!(resultBag.Equals(null)))
        {
            bool result = resultBag.AddItemToShortCut(index, item);

            if (result)
            {
                RefreshShortCutByItem(item);
                return result;
            }
        }
        return false;
    }
    public bool ChangeShortCutPosition(GenericItemScriptable itemChanged, int index)
    {
        BagScriptable resultBag = CastGenericBagToBag();

        if (!(resultBag.Equals(null)))
        {
            bool result = resultBag.ChangeItemPosition(itemChanged, index);
            if (result)
            {
                StartCoroutine("RefreshShortCutView");
                return true;
            }
        }
        return false;
    }
    public bool RemoveItemFromShortCut(int id)
    {
        BagScriptable resultBag = CastGenericBagToBag();

        if (!(resultBag.Equals(null)))
        {
            bool result = resultBag.RemoveItemFromShortCutById(id);
            if (result)
            {
                StartCoroutine("RefreshShortCutView");//This statment call the shortcut UI refresh
                return true;
            }
        }
        return false;
    }
    private void UseShortCutToUseItem(int index)
    {
        if (!GameManager.Instance.inventoryIsOpen)
        {
            BagScriptable resultBag = CastGenericBagToBag();
            GenericItemScriptable resultItem = resultBag.GetItemByIndexPosition(index);

            if (resultItem != null) UseItem(resultItem.Id, 1);
            StartCoroutine("RefreshInventoryView");
        }
    }
    private IEnumerator RefreshShortCutView()//This method refresh the shortcut UI view
    {
        yield return new WaitForSeconds(0.02f);
        BagScriptable resultBag = CastGenericBagToBag();
        Dictionary<int, GenericItemScriptable> resultDictionary = resultBag.ItemsShortCutDictionary;
        ShortCutView.Instance.RefreshSlotSystem(resultDictionary);
    }
    private void RefreshShortCutByItem(GenericItemScriptable itemUpdate)//This method refresh the especific item that is on a shortcut
    {
        BagScriptable resultBag = CastGenericBagToBag();
        List<int> idsResults = resultBag.GetIdsFromItemShortCutDictionary();

        foreach (var id in idsResults)
        {
            if (id == itemUpdate.Id)
            {
                Dictionary<int, GenericItemScriptable> resultDictionary = resultBag.ItemsShortCutDictionary;
                List<int> keyResults = resultBag.GetUsedKeysFromShortCutDictionary();
                ShortCutView.Instance.UpdateSlot(resultDictionary, keyResults);
            }
        }
    }
    public void InventoryStateChanged() => StartCoroutine("RefreshInventoryView");
    #endregion

    #region - Helpers Methods -

    private BagScriptable CastGenericBagToBag()
    {
        BagScriptable resultBag = ScriptableObject.CreateInstance<BagScriptable>();

        if (currentBag is BagScriptable) resultBag = (BagScriptable)currentBag;

        return resultBag;
    }
    #endregion
}