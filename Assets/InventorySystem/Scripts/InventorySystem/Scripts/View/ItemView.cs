using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    #region - Item Declaration -
    [SerializeField] private int quantity;
    [SerializeField] private GenericItemScriptable item;
    [SerializeField] private List<GenericActionScriptable> actionList;
    private ActionManagerEvent actionManagerEvt;
    #endregion

    #region - Data Get and Set -
    public int Quantity { get => quantity; set => quantity = value; }
    public GenericItemScriptable Item { get => item; }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) Collect();   
    }
    private void Collect()
    {
        bool result = InventoryManagerController.Instance.AddItemToCurrentBag(item, quantity, false);

        if (result)
        {
            actionManagerEvt = new ActionManagerEvent();
            actionManagerEvt.DispatchAllGenericActionListEvent(actionList);
            Destroy(this.gameObject);
        }
    }
    private void PrepareObject()
    {
        if (!gameObject.GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        if (!gameObject.GetComponent<SphereCollider>())
        {
            gameObject.AddComponent<SphereCollider>().isTrigger = true; gameObject.GetComponent<SphereCollider>().radius = 2.2f;
        }
        if (!gameObject.GetComponent<MeshCollider>()) gameObject.AddComponent<MeshCollider>().convex = true;
    }
    private void OnValidate() => PrepareObject();
}