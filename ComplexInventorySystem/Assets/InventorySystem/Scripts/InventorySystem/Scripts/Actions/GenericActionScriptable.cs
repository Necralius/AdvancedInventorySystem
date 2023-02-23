using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericActionScriptable : ScriptableObject
{
    [SerializeField, Range(0, 30)] private float delayToStart;

    protected float DelayToStart { get => delayToStart; }

    public abstract IEnumerator Execute();
}