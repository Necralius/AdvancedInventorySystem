using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlaySoundAction", menuName = "InventorySystem/Action/NewPlaySoundAction")]
public class PlaySoundActionScriptable : GenericActionScriptable
{
    //Code made by Victor Paulo Melo da Silva and a Advanced Inventory course used as an base  - https://www.linkedin.com/in/victor-nekra-dev/
    //PlaySoundActionScriptable - Code Update Version 0.3 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Action Data -
    [SerializeField] private AudioClip audioFile;
    #endregion

    #region - Play Sound Action Execution -
    public override IEnumerator Execute()//This method represents the play sound action execution
    {
        yield return new WaitForSeconds(DelayToStart);

        if (audioFile != null) GameController.Instance.PlayAudio(audioFile);
        else Debug.LogWarning("There is no a valid audio file in this play sound asset.");
    }
    #endregion
}