using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour
{
    public IntVariable score;

    public void Reset()
    {
        score.RuntimeValue = 0;
    }
}
