using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Game Events hold lists of actions to take once the event occurs
[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    protected List<UnityEvent> events = new List<UnityEvent>();
    protected List<UnityAction> actions = new List<UnityAction>();

    //Calls the event raised function of each registered listener
    public void Raise()
    {
        #if UNITY_EDITOR
        Debug.Log("Raised game event: "+this.name);
        #endif

        for(int i = events.Count - 1; i >= 0; i--)
        {
            events[i].Invoke();
        }

        for(int i = actions.Count - 1; i >= 0; i--)
        {
            actions[i].Invoke();
        }    
    }

    //adds a listener
    public void RegisterListener(UnityEvent action)
    {
        events.Add(action);
    }

    //removes a listener
    public void UnregisterListener(UnityEvent action)
    {
        events.Remove(action);
    }

    public void RegisterListener(UnityAction action)
    {
        actions.Add(action);
    }

    public void UnregisterListener(UnityAction action)
    {
        actions.Remove(action);
    }

}

