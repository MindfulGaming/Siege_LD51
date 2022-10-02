using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowExplainer : MonoBehaviour
{
    public GameObject explainer;
    public TextMeshPro textSymbol;
    
    public void Switch()
    {
        if(explainer.gameObject.activeSelf)
        {
            explainer.gameObject.SetActive(false);
            textSymbol.text = "?";
        }

        else
        {
            explainer.gameObject.SetActive(true);
            textSymbol.text = "X";
        }
    }
}
