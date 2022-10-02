using System;
using System.Collections.Generic;
using UnityEngine;

//A scriptable object that represents a type of Input Event, eg Tap, Swipe, Raise.
//It contains a list of actions which are invoked when raised, passing on touchData

[CreateAssetMenu(menuName = "Events/InputEvent")]
public class InputEvent : ScriptableObject
{
    protected List<Action<TouchData>> actions = new List<Action<TouchData>>();
    int currentIndex = 0;

    public void Raise(TouchData touchData)
    {
        for (currentIndex = actions.Count - 1; currentIndex >= 0; currentIndex--) 
        {
            if(currentIndex < actions.Count) actions[currentIndex].Invoke(touchData);
        }
        currentIndex = 0;
    }

    public void RegisterListener(Action<TouchData> action)
    {
        actions.Add(action);
    }

    public void UnregisterListener(Action<TouchData> action)
    {
        int i = actions.FindIndex(a => action == a); //find index of object
        actions.RemoveAt(i); //remove it
        if(i < currentIndex) currentIndex --; //if it was below the current index
    }



}
