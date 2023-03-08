using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChangeCharPropertiesAction", menuName = "InventorySystem/Action/ChangeCharPropertiesAction")]
public class ChangeCharPropertiesActionScriptable : GenericActionScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //ChangeCharPropertiesActionScriptable - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action Data -
    [SerializeField, Range(-100f, 100f)] private float life;
    [SerializeField, Range(-100f, 100f)] private float thirst;
    [SerializeField, Range(-100f, 100f)] private float hungry;
    [SerializeField, Range(-100f, 100f)] private float stamina;
    [SerializeField, Range(10f, 30f)] private float speed;

    [SerializeField] private bool applyNewPosition;
    [SerializeField] private Transform teleportToPos;

    [SerializeField] private bool resetSpeed;
    #endregion

    #region - Action Execution Method -
    public override IEnumerator Execute()//This method execute the change char action
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

        if (applyNewPosition) GameController.Instance.TeleportPlayer(teleportToPos);
        yield return null;
    }
    #endregion
}