using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    public float InitialValue;
    public float RuntimeValue;
    
    //after Unity deserializes the initial value is used, to prevent editing the initial value saved to disk
    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnEnable()
    {
        RuntimeValue = InitialValue;
    }
}
