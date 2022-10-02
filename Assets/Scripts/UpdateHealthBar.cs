using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class UpdateHealthBar : MonoBehaviour
{
    public Rectangle fillBar;
    public float minLength;
    public float maxLength;
    public CastlePiece piece;
    
    void Update()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        float t = piece.health / piece.startingHealth;
        fillBar.Width = Mathf.Lerp(0f, maxLength, t);
        fillBar.Width = Mathf.Max(minLength, fillBar.Width);
    }
}
