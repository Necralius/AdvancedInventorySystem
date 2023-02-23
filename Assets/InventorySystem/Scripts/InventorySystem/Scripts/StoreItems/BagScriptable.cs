using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBag", menuName = "InventorySystem/Store Items/New Bag")]
public class BagScriptable : GenericBagScriptable
{

    protected override void ResetBag()
    {
        base.ResetBag();

        matrixUtility = new MatrixUtility(maxRow, maxColumn, title);

    }

}