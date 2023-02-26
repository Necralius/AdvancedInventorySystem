using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManagerController : MonoBehaviour
{
    #region - Singleton Pattern -
    public static InventoryManagerController Instance;
    void Awake() => Instance = this;
    #endregion

    #region - Data Declaration - 
    [SerializeField] private GameObject playerObject;
    [SerializeField] GameObject directionToLaunchItem;
    [SerializeField] GameObject placeToDrop;

    [SerializeField] List<GameObject> itemList;

    [SerializeField] GenericBagScriptable currentBag;
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

        if (true)//Exception Case Verification
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

            if (itemResult.CurrentQuantity == 0 && itemResult.RemoveWhenNumberIsZero)
            {
                DropItem(itemResult);
            }
        }
    }
    
    public void OrganizeItem()
    {

    }
    public bool RemoveItem()
    {
        return true;
    }
    public bool DropItem(GenericItemScriptable item)
    {
        bool result = false;
        int currentQuantity;

        if (item.Id >= 0)
        {
            currentQuantity = item.CurrentQuantity;
            result = currentBag.DropItem(item.Id);

            if (result)
            {
                StartCoroutine("RefreshInventoryView");

                BagScriptable resultBag = CastGenericBagToBag();

                List<int> idList = resultBag.GetIdsFromItemShortCutDictionary();

                foreach (var idShortCut in idList)
                {
                    if (idShortCut == item.Id) RemoveItemFromShortCut(item.Id);
                }

                BuildMeshModel(item.Id, currentQuantity);
                return true;
            }
        }

        return true;
    }
    private void BuildMeshModel(int id, int currentQuantity)
    {
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
    private IEnumerator RefreshInventoryView()
    {
        yield return new WaitForSeconds(0.02f);
        List<GenericItemScriptable> resultList = currentBag.ReturnFullList();
        InventoryView.Instance.UpdateAllItems(resultList);
    }
    #endregion


    #region - OnDropItem and OnPointDownItem -
    public bool OnDropItem(GenericItemScriptable itemDrop, GameObject origin, Vector2 coordinate, SlotPlaceTo slotPlaceTo)
    {
        int index = (int)coordinate.y;
        if (origin.transform.parent.parent.name == "img_GridBackground" && slotPlaceTo != SlotPlaceTo.BAG)
        {
            if (slotPlaceTo == SlotPlaceTo.SHORT_CUT) return AddItemToCurrentShortCut(index, itemDrop);
        }

        //ShortCur => Change for another position

        if (origin.transform.parent.parent.name == "cont_ShortCutSlotsGroup" && slotPlaceTo == SlotPlaceTo.SHORT_CUT)
        {
            return ChangeShortCutPosition(itemDrop, index);
        }
        return false;
    }

    public bool OnDropItem(GenericItemScriptable itemDrop, string originTag)
    {
        //ItemDrop come from inventory

        if (originTag.Equals("ComplexSlot")) return DropItem(itemDrop);
        if (originTag.Equals("SpecialSlot")) return RemoveItemFromShortCut(itemDrop.Id);

        if (originTag.Equals("ClothingWeaponsSlot"))
        {

        }
        return false;
    }
    public void OnPointerDownItem(GenericItemScriptable item, GameObject origin)
    {
        if (origin.transform.parent.parent.name == ("img_GridBackground"))
        {
            InventoryView.Instance.UpdateDescriptionPanel(item);
        }
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
                StartCoroutine("RefreshShortCutView");
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
            else Debug.LogWarning("Do not exists any item in this short cut slot!");
            StartCoroutine("RefreshInventoryView");
        }
    }
    private IEnumerator RefreshShortCutView()
    {
        yield return new WaitForSeconds(0.02f);
        BagScriptable resultBag = CastGenericBagToBag();
        Dictionary<int, GenericItemScriptable> resultDictionary = resultBag.ItemsShortCutDictionary;
        ShortCutView.Instance.RefreshSlotSystem(resultDictionary);
    }

    private void RefreshShortCutByItem(GenericItemScriptable itemUpdate)
    {
        BagScriptable resultBag = CastGenericBagToBag();
        List<int> idsResults = resultBag.GetIdsFromItemShortCutDictionary();

        foreach(var id in idsResults)
        {
            if(id == itemUpdate.Id)
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