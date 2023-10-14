using UnityEngine;

public static partial class Events
{
    public static readonly GameEvent<float> OnDamage = new();
    public static readonly GameEvent<GameObject> OnPickup = new();
    public static readonly GameEvent OnLoseGame = new();
    public static readonly GameEvent OnWinLevel = new();
    public static readonly GameEvent<MessageType, string> OnMessage = new();
    
    // Internal events
    public static readonly GameEvent<RoadController> OnRemoveFromRoadList = new();
}