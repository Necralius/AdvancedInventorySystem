using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericActionScriptable : ScriptableObject //This class represent the action main model
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //PopupActionScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action Data -
    [SerializeField, Range(0, 30)] private float delayToStart;
    protected float DelayToStart { get => delayToStart; }
    #endregion

    #region - Action Abstract Method Implementation -
    public abstract IEnumerator Execute();//This method represents the abstract Execute method that all inherited classes will have and will need to implement
    #endregion
}