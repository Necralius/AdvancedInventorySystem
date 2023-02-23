using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Update()
    {
        if (Input.GetKeyDown(keyCodeShortcutList[0]))
        {
            
        }
        if (Input.GetKeyDown(keyCodeShortcutList[1]))
        {

        }
        if (Input.GetKeyDown(keyCodeShortcutList[2]))
        {

        }
        if (Input.GetKeyDown(keyCodeShortcutList[3]))
        {

        }
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

                //Send a List => InventoryView (resultItemList)
            }

            if (currentBag.UsedOrganizeBtSizePriority)
            {
                List<GenericItemScriptable> resultItemList = currentBag.ReturnFullList();

                //Send a List => InventoryView (resultItemList)
                currentBag.UsedOrganizeBtSizePriority = false;
            }
            //Refresh Shortcut
        }

        return result;
    }

    private void UseItem(int id, int quantity)
    {
        bool result = currentBag.UseItem(id, quantity);

        if (result)
        {
            GenericItemScriptable itemResult = currentBag.FindItemById(id);
            //Refresh
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
                //View Updates

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
    #endregion
}