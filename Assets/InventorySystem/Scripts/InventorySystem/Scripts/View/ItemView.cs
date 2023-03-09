using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ItemView - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Item Declaration -
    [SerializeField] private int quantity;
    [SerializeField] private GenericItemScriptable item;
    [SerializeField] private List<GenericActionScriptable> actionList;
    private ActionManagerEvent actionManagerEvt;
    #endregion

    #region - Getting and Set Data -
    public int Quantity { get => quantity; set => quantity = value; }
    public GenericItemScriptable Item { get => item; }
    #endregion

    #region - Item Collect System -
    private void OnTriggerEnter(Collider other)//This method detect if the player has collided with the item trigger collider and if it has, the item is collected
    {
        if (other.transform.CompareTag("Player")) Collect();   
    }
    private void Collect()//This method collect the holded item, and storage it in the player's current bag
    {
        bool result = InventoryManagerController.Instance.AddItemToCurrentBag(item, quantity, false);

        if (result)//This statement dispatch and execute all action events on the list
        {
            actionManagerEvt = new ActionManagerEvent();
            actionManagerEvt.DispatchAllGenericActionListEvent(actionList);
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region - Item Preparation -
    private void PrepareObject()//This method assists the developer to make new items prefabs, the method automatically add all needed components to the item and configure the the components
    {
        if (!gameObject.GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        if (!gameObject.GetComponent<SphereCollider>())
        {
            gameObject.AddComponent<SphereCollider>().isTrigger = true; gameObject.GetComponent<SphereCollider>().radius = 2.2f;
        }
        if (!gameObject.GetComponent<MeshCollider>()) gameObject.AddComponent<MeshCollider>().convex = true;
    }
    private void OnValidate() => PrepareObject();
    #endregion
}