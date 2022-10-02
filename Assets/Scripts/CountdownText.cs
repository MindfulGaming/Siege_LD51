using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CountdownText : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float durationInSeconds;
    public GameEvent onCountDownFinished;
    public bool startOnAwake;
    public GameEvent startOnEvent;
    public bool disableGameObjectOnFinish;
    public enum ZeroFormat {zero, blank, dash};
    public ZeroFormat zeroFormat;

    void Awake()
    {
        textMesh.text = durationInSeconds.ToString();
    }
    
    void OnEnable()
    {
        if(startOnAwake) StartCountdown();
        if(startOnEvent != null) startOnEvent.RegisterListener(StartCountdown);
    }

    void OnDisable()
    {
        if(startOnEvent != null) startOnEvent.UnregisterListener(StartCountdown);
    }
    
    void StartCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(Countdown(durationInSeconds)); 
    }

    void OnCountDownEnd()
    {
        StartCountdown();
        if(onCountDownFinished != null) onCountDownFinished.Raise();
        if(disableGameObjectOnFinish) this.gameObject.SetActive(false);
    }

    void SetZero()
    {
        switch(zeroFormat)
        {
            case ZeroFormat.zero:
            {
                textMesh.text = "0";
                break;
            }
            case ZeroFormat.blank:
            {
                textMesh.text = "";
                break;
            }
            case ZeroFormat.dash:
            {
                textMesh.text = "-";
                break;
            }
        }
    }

    IEnumerator Countdown(float duration)
    {
        float secondsRemaining = duration  + 0.999f; //add 0.999 so we show the overall length for (almost) a full second
        while(secondsRemaining >= 1f) //then show zero after reaching 1s mark so overall length of time still correct
        {
            if(secondsRemaining < 60f) textMesh.text = TimeSpan.FromSeconds(secondsRemaining).ToString(@"%s");
            else textMesh.text = TimeSpan.FromSeconds(secondsRemaining).ToString(@"m\:s");
            secondsRemaining -= Time.deltaTime;
            yield return null;
        }
        SetZero();
        OnCountDownEnd();
    }
}
