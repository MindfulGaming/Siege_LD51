using System;
using UnityEngine.Events;

//Extends Unit Events to pass TouchData to the functions being invoked by it
[Serializable]
public class UnityInputEvent : UnityEvent<TouchData> { }