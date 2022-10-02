using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    public TextMeshPro textMesh;
    public IntVariable score;

    void Update()
    {
        textMesh.text = score.RuntimeValue.ToString();
    }
}
