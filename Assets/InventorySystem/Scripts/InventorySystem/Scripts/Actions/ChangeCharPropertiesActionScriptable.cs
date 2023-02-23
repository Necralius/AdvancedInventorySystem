using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChangeCharPropertiesAction", menuName = "InventorySystem/Action/ChangeCharPropertiesAction")]
public class ChangeCharPropertiesActionScriptable : GenericActionScriptable
{

    #region - Main Declaration -
    [SerializeField, Range(-100f, 100f)] private float life;
    [SerializeField, Range(-100f, 100f)] private float thirsty;
    [SerializeField, Range(-100f, 100f)] private float hungry;
    [SerializeField, Range(-100f, 100f)] private float diseaseLevel;
    [SerializeField, Range(10f, 30f)] private float speed;

    [SerializeField] private bool applyNewPosition;
    [SerializeField] private Vector3 teleportToPos;

    [SerializeField] private bool resetSpeed;
    #endregion

    #region - Methods -
    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(DelayToStart);

        if (life != 0)
        {
            //GameControler => Function();
        }

        if (thirsty != 0)
        {
            //GameControler => Function();
        }

        if (hungry != 0)
        {
            //GameControler => Function();
        }

        if (diseaseLevel != 0)
        {
            //GameControler => Function();
        }

        if (speed != 0)
        {
            //GameControler => Function();
        }

        if (applyNewPosition)
        {
            //GameControler => Function();
        }
    }

    #endregion
}