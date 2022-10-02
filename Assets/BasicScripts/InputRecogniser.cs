using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/*
Input Recogniser uses the Unity Input System and EnhancedTouchSupport API to recognise gestures. It 
then calls Raise(TouchData) on the relevant InputEvent, which will tell all registered InputEventListener 
components to invoke their response
*/
public class InputRecogniser : MonoBehaviour
{
    public InputEvent swipe;
    public InputEvent tap;
    public InputEvent shortTap;
    public InputEvent longTap;
    public InputEvent drag;
    public InputEvent touchDown;
    public InputEvent release;

    //params
    public float minSwipeDistance;
    public float longPressDuration;
    public bool logEvents;

    Vector2 firstClickPosition;
    float mouseDownTime;
    bool mouseDown;
    TouchData touchData;
    UnityEngine.InputSystem.Mouse mouse;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
        touchData = new TouchData();
        mouse = UnityEngine.InputSystem.Mouse.current;
    }

    void Update()
    {
        if(Touch.activeTouches.Count > 0)
        {
            HandleTouchInput();
        }
        else HandleMouseInput();

    }

    void HandleMouseInput()
    {
        if(mouse.leftButton.wasPressedThisFrame)
        {
            firstClickPosition = mouse.position.ReadValue();
            touchData.startPosition = Camera.main.ScreenToWorldPoint(firstClickPosition);
            touchDown.Raise(touchData);
            if(logEvents) Debug.Log("mouse touch down event raised");
            mouseDownTime = Time.time;
            mouseDown = true;
        }

        else if(mouse.leftButton.wasReleasedThisFrame)
        {
            mouseDown = false;
            touchData.endPosition = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            touchData.duration = Time.time - mouseDownTime;
            release.Raise(touchData);
            if(logEvents) Debug.Log("mouse relase event raised");

            Vector2 swipeVector = mouse.position.ReadValue() - firstClickPosition;
            if(swipeVector.magnitude > minSwipeDistance)
            {
                swipe.Raise(touchData);
                if(logEvents) Debug.Log("mouse swipe event raised");
            }
            else
            {
                tap.Raise(touchData); //any tap - short or long
                if(logEvents) Debug.Log("mouse tap event raised");

                if(touchData.duration >= longPressDuration)
                {
                    longTap.Raise(touchData);
                    if(logEvents) Debug.Log("mouse long tap event raised");
                }
                else 
                {
                    shortTap.Raise(touchData);
                    if(logEvents) Debug.Log("mouse short tap event raised");
                }
            }
        }

        else if(mouse.leftButton.isPressed)
        {
            touchData.endPosition = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            touchData.duration = Time.time - mouseDownTime;
            drag.Raise(touchData);
        }
    }

    void HandleTouchInput()
    {
        //track the first finger that touched the screen
        Touch t = Touch.activeTouches[0];
        touchData.startPosition = Camera.main.ScreenToWorldPoint(t.startScreenPosition);
        touchData.endPosition = Camera.main.ScreenToWorldPoint(t.screenPosition);
        touchData.duration = (float)(t.time - t.startTime);

        switch (t.phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:
            {
                touchDown.Raise(touchData);
                if(logEvents) Debug.Log("touch down event raised: "+gameObject.GetInstanceID().ToString());
                break;
            }
            
            case UnityEngine.InputSystem.TouchPhase.Ended:
            case UnityEngine.InputSystem.TouchPhase.Canceled:
            {
                Vector2 swipeVector = t.screenPosition - t.startScreenPosition;
                if(swipeVector.magnitude > minSwipeDistance)
                {
                    swipe.Raise(touchData);
                    if(logEvents) Debug.Log("swipe event raised");
                }
                else
                {
                    tap.Raise(touchData); //any tap - short or long
                    if(logEvents) Debug.Log("tap event raised");

                    if(touchData.duration >= longPressDuration)
                    {
                        longTap.Raise(touchData);
                        if(logEvents) Debug.Log("long tap event raised");
                    }
                    else
                    {
                        shortTap.Raise(touchData);
                        if(logEvents) Debug.Log("short tap event raised");
                    }
                }

                release.Raise(touchData);
                if(logEvents) Debug.Log("relase event raised");
                break;
            }
            
            case UnityEngine.InputSystem.TouchPhase.Stationary:
            case UnityEngine.InputSystem.TouchPhase.Moved:
            {
                drag.Raise(touchData);
                break;
            }
        }
    }

        /*
        else if (Input.GetMouseButtonDown(0))
        {
            firstClickPosition = Input.mousePosition;
            touchData.startPosition = Camera.main.ScreenToWorldPoint(firstClickPosition);
            touchDown.Raise(touchData);
            if(logEvents) Debug.Log("mouse touch down event raised");
            mouseDownTime = Time.time;
            mouseDown = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            touchData.endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchData.duration = Time.time - mouseDownTime;
            release.Raise(touchData);
            if(logEvents) Debug.Log("mouse relase event raised");

            Vector2 swipeVector = (Vector2)Input.mousePosition - firstClickPosition;
            if(swipeVector.magnitude > minSwipeDistance)
            {
                swipe.Raise(touchData);
                if(logEvents) Debug.Log("mouse swipe event raised");
            }
            else
            {
                tap.Raise(touchData); //any tap - short or long
                if(logEvents) Debug.Log("mouse tap event raised");

                if(touchData.duration >= longPressDuration)
                {
                    longTap.Raise(touchData);
                    if(logEvents) Debug.Log("mouse long tap event raised");
                }
                else 
                {
                    shortTap.Raise(touchData);
                    if(logEvents) Debug.Log("mouse short tap event raised");
                }
            }
        }

        else if(mouseDown)
        {
            touchData.endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchData.duration = Time.time - mouseDownTime;
            drag.Raise(touchData);
        }
    }
    */

}
  

