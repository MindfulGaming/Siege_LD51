using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAndDisableObjects : MonoBehaviour
{
    public GameEvent triggerEvent;
    public List<GameObject> objectsToEnable;
    public List<GameObject> objectsToDisable;
    public bool tryFading;
    public float fadeInDuration;
    public float timeBeforeFadeIn;
    public float fadeOutDuration;
    
    void OnEnable()
    {
        if(triggerEvent != null) triggerEvent.RegisterListener(EnableAndDisable);
    }

    void OnDisable()
    {
        if(triggerEvent != null) triggerEvent.UnregisterListener(EnableAndDisable);
    }
    
    public void EnableAndDisable()
    {
        EnableObjects();
        DisableObjects();
    }
    
    public void EnableObjects()
    {
        foreach(GameObject g in objectsToEnable)
        {
            if(tryFading)
            {
                FadeInRenderGroup fadeIn = g.GetComponent<FadeInRenderGroup>();
                if(fadeIn != null) StartCoroutine(WaitThenFadeIn(fadeIn, timeBeforeFadeIn, fadeInDuration));
                else g.SetActive(true);
            }
            else g.SetActive(true);
        }
    }

    private IEnumerator WaitThenFadeIn(FadeInRenderGroup group, float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);
        group.gameObject.SetActive(true);
        group.StartAnimation(duration);
    }

    public void DisableObjects()
    {
        foreach(GameObject g in objectsToDisable)
        {
            if(tryFading)
            {
                FadeOutRenderGroup fadeOut = g.GetComponent<FadeOutRenderGroup>();
                if(fadeOut != null)
                {
                    fadeOut.StartAnimation(fadeOutDuration);
                    StartCoroutine(WaitThenDisable(fadeOutDuration, g));
                }
                
                else g.SetActive(false);
            }
            else g.SetActive(false);
        }
    }

    IEnumerator WaitThenDisable(float duration, GameObject gameObject)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
