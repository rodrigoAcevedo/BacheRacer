using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int TotalKilometersRan;
    public float LastKnownPlayerHealth;

    [SerializeField] public GameParameters BaseParameters;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Events.OnDataReceived.Subscribe(OnDataReceived);
        Events.OnInventoryDataReceived.Subscribe(OnInventoryDataReceived);
    }

    private void OnDisable()
    {
        Events.OnDataReceived.Unsubscribe(OnDataReceived);
        Events.OnInventoryDataReceived.Unsubscribe(OnInventoryDataReceived);

    }

    private void Start()
    {
        PlayfabManager.Instance.Login(GetUserData);
    }

    private void GetUserData()
    {
        UserDataUtility.GetUserDataFromServer();
        PlayfabManager.Instance.GetUserInventory();
    }

    private void OnDataReceived()
    {
        LastKnownPlayerHealth = UserDataUtility.GetPlayerHealth();
        TotalKilometersRan = UserDataUtility.GetPlayerKilometersRan();
    }

    private void OnInventoryDataReceived()
    {
        Events.OnUpdateMenuStats.Dispatch();
    }
    

}
