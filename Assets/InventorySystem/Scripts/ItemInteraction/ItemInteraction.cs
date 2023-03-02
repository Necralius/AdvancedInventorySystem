using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    #region - DebugGrab -
    public float grabRadius;
    #endregion

    public GameObject grabArea;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(grabArea.transform.position, grabRadius * 0.5f);
        Gizmos.DrawSphere(grabArea.transform.position, grabRadius * 0.5f);
    }
}