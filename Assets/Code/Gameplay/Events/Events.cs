using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public static partial class Events
{
    public static readonly GameEvent<float> OnDamage = new GameEvent<float>();
    public static readonly GameEvent<GameObject> OnPickup = new GameEvent<GameObject>();
    public static readonly GameEvent OnLoseGame = new GameEvent();
    public static readonly GameEvent OnWinLevel = new GameEvent();
    
    // UI related events.
    public static readonly GameEvent<MessageType, string> OnMessage = new GameEvent<MessageType, string>();
    public static readonly GameEvent<string> OnUpdatePlayerHealth = new GameEvent<string>();
    
    // Internal events
    public static readonly GameEvent<RoadController> OnRemoveFromRoadList = new GameEvent<RoadController>();
    public static readonly GameEvent OnUpdateMenuStats = new GameEvent();
    
    // Playfab events
    public static readonly GameEvent OnDataReceived = new GameEvent();
    public static readonly GameEvent OnInventoryDataReceived = new GameEvent();
}