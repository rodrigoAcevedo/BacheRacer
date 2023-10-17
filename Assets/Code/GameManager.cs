using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int TotalKilometersRan;
    public float LastKnownPlayerHealth;
    public int CoinsAmount;
    public int DiamondsAmount;

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
        Debug.Log(UserDataUtility.Data);
        LastKnownPlayerHealth = UserDataUtility.GetPlayerHealth();
        TotalKilometersRan = UserDataUtility.GetPlayerKilometersRan();
        Events.OnUpdateMenuStats.Dispatch();
    }

    private void OnInventoryDataReceived()
    {
        CoinsAmount = InventoryUtility.Coins;
        DiamondsAmount = InventoryUtility.Diamonds;
        Events.OnUpdateMenuStats.Dispatch();
    }
    

}
