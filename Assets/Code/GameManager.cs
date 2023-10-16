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
    }

    private void OnDisable()
    {
        Events.OnDataReceived.Unsubscribe(OnDataReceived);
    }

    private void Start()
    {
        PlayfabManager.Instance.Login(GetUserData);
    }

    private void GetUserData()
    {
        UserDataUtility.GetUserDataFromServer();
    }

    private void OnDataReceived()
    {
        Debug.Log(UserDataUtility.Data);
        LastKnownPlayerHealth = UserDataUtility.GetPlayerHealth();
        TotalKilometersRan = UserDataUtility.GetPlayerKilometersRan();
        // Is this the best place for dispatch this call?
        Events.OnUpdateMenuStats.Dispatch();
    }
    

}
