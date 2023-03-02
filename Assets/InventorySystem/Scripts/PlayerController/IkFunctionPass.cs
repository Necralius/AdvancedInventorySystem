using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkFunctionPass : MonoBehaviour
{
    public void ObjectGrabbed() => PlayerController.Instance.OnItemGrabbed();
    public void CollectGrabbedItem() => PlayerController.Instance.CollectGrabbedItem();
}