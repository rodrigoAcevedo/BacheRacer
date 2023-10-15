using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance { get; private set; }
    
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

    public void SaveData(string property, string value)
    {
        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { property, value }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSent, OnDataSentError);
    }

    public void GetData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnDataReceivedError);
    }
    private void OnDataSent(UpdateUserDataResult result)
    {
        // TODO: Complete
    }

    private void OnDataSentError(PlayFabError error)
    {
        // TODO: Complete
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Received user data!");
        if (result.Data != null)
        {
            // TODO: See how to handle data that we receive from server.
        }
    }

    private void OnDataReceivedError(PlayFabError error)
    {
        // TODO: Complete
    }
    
    private void Start()
    {
        Login();
    }

    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Succesful login/account create!");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while login in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }
}
