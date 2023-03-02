using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChangeCharPropertiesAction", menuName = "InventorySystem/Action/ChangeCharPropertiesAction")]
public class ChangeCharPropertiesActionScriptable : GenericActionScriptable
{

    #region - Main Declaration -
    [SerializeField, Range(-100f, 100f)] private float life;
    [SerializeField, Range(-100f, 100f)] private float thirst;
    [SerializeField, Range(-100f, 100f)] private float hungry;
    [SerializeField, Range(-100f, 100f)] private float stamina;
    [SerializeField, Range(10f, 30f)] private float speed;

    [SerializeField] private bool applyNewPosition;
    [SerializeField] private Vector3 teleportToPos;

    [SerializeField] private bool resetSpeed;
    #endregion

    #region - Methods -
    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(DelayToStart);

        if (hungry != 0)
        {
            if (thirst != 0)
            {
                if (stamina != 0)
                {
                    GameController.Instance.EatFood(hungry, thirst, stamina);
                    yield return null;
                }
                GameController.Instance.EatFood(hungry, thirst, 0);
                yield return null;
            }
            GameController.Instance.EatFood(hungry, 0, 0);
            yield return null;
        }

        if (life != 0)
        {
            if (life > 0) GameController.Instance.CurePlayer(life);
            else if (life < 0) GameController.Instance.DamagePlayer(life);
        }

        if (applyNewPosition)
        {
            //GameControler => Function();
        }
    }

    #endregion
}