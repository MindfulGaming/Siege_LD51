using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SingleButton : UIButton
{
    public InputEvent inputTrigger;
    public GameEvent triggerGameEvent;
    public UnityEvent triggerEvent;
    public bool loadScene;
    public string sceneName;
    [Tooltip("If set, the button can only be pressed when this variable is true")]
    public BoolVariable selectableVariable;
    public bool limitToOnePress;
    private bool hasBeenPressed;
    [Header("Animation")]
    public bool animateBeforeTrigger;
    public bool animateAfterTrigger;
    [Tooltip("A value of 1.1 gives a 10% increase")]
    public float sizeIncreaseFactor;
    public EasingFunctions.EasingFunction easingFunction;
    public float animationDuration;
    
    void OnEnable()
    {
        Debug.Log("Button: "+this.name+" enabled");
        if(inputTrigger != null) inputTrigger.RegisterListener(OnInput);
        Reset();
    }

    public void Reset()
    {
        hasBeenPressed = false;
    }

    void OnDisable()
    {
        if(inputTrigger != null) inputTrigger.UnregisterListener(OnInput);
    }

    void OnInput(TouchData touchData)
    {
        if(!limitToOnePress || limitToOnePress && !hasBeenPressed) //unlimited, or limited but not pressed
        {
            if((selectableVariable == null || selectableVariable.RuntimeValue) && CheckIfSelected(touchData.endPosition)) //selectable and input in correct place
            {
                //Debug.Log("Pressed Button: "+this.gameObject.name);
                if(animateBeforeTrigger) StartCoroutine(ButtonAnimation());
                else if(animateAfterTrigger) StartCoroutine(ButtonAnimationAfterSelected());
                else OnSelected();
            }
        }
    }

    public override void OnSelected()
    {
       if(triggerGameEvent != null) triggerGameEvent.Raise();
       if(triggerEvent != null) triggerEvent.Invoke();
       hasBeenPressed = true;
       if(loadScene) SceneManager.LoadSceneAsync(sceneName);
    }

    public override void OnDeselected()
    {
        
    }

    IEnumerator ButtonAnimation()
    {
        float startSize = transform.localScale.x;
        float endSize = transform.localScale.x * sizeIncreaseFactor;
        float timeElapsed = 0;
        float t;
        float duration = animationDuration / 2;
        while (timeElapsed < duration) //grow
        {
            t = timeElapsed / duration;
            t = EasingFunctions.DoEase(easingFunction, t);
            var scaleFactor = Mathf.Lerp(startSize, endSize, t);
            transform.localScale = Vector3.one * scaleFactor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        timeElapsed = 0;
        while (timeElapsed < duration) //shrink
        {
            t = timeElapsed / duration;
            t = EasingFunctions.DoEase(easingFunction, t);
            var scaleFactor = Mathf.Lerp(endSize, startSize, t);
            transform.localScale = Vector3.one * scaleFactor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * startSize;

        //select button on animation end
        OnSelected();
    }

    IEnumerator ButtonAnimationAfterSelected()
    {
        OnSelected();

        float startSize = transform.localScale.x;
        float endSize = transform.localScale.x * sizeIncreaseFactor;
        float timeElapsed = 0;
        float t;
        float duration = animationDuration / 2;
        while (timeElapsed < duration) //grow
        {
            t = timeElapsed / duration;
            t = EasingFunctions.DoEase(easingFunction, t);
            var scaleFactor = Mathf.Lerp(startSize, endSize, t);
            transform.localScale = Vector3.one * scaleFactor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        timeElapsed = 0;
        while (timeElapsed < duration) //shrink
        {
            t = timeElapsed / duration;
            t = EasingFunctions.DoEase(easingFunction, t);
            var scaleFactor = Mathf.Lerp(endSize, startSize, t);
            transform.localScale = Vector3.one * scaleFactor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * startSize;
    }
}
