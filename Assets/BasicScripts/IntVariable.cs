using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
    public int InitialValue;
    public int RuntimeValue;

    public void OnEnable()
    {
        RuntimeValue = InitialValue;
    }


    //after Unity deserializes the initial value is used, to prevent editing the initial value saved to disk
    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

}
