using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlaySoundAction", menuName = "InventorySystem/Action/NewPlaySoundAction")]
public class PlaySoundActionScriptable : GenericActionScriptable
{
    [SerializeField] private AudioClip audioFile;



    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(DelayToStart);

        if (audioFile != null)
        {
            //Game Controller => PlayAudio();
        }
        else Debug.LogWarning("There is no a valid audio file in this play sound asset.");
    }
}
