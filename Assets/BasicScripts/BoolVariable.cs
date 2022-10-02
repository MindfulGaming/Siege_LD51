using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Bool")]
public class BoolVariable : ScriptableObject
{
    public bool InitialValue;
    public bool RuntimeValue;

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnEnable()
    {
        RuntimeValue = InitialValue;
    }
}
