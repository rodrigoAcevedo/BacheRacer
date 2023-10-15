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
    private void OnDataSent(UpdateUserDataResult result)
    {
        
    }

    private void OnDataSentError(PlayFabError error)
    {
        
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
