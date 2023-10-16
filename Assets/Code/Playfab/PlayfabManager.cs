using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance { get; private set; }
    
    // To be honest I'm not so happy doing this but I need somehow have a generic way to retrieve/use data without crossing references on this class.
    private Action OnLoginSucessCallback;
    
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
        Debug.Log("Data successfully saved!");
    }

    private void OnDataSentError(PlayFabError error)
    {
        Debug.Log("Error while saving data to server");
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        Debug.Log("Received user data!");
        if (result.Data != null)
        {
            UserDataUtility.Data = result.Data;
            Events.OnDataReceived.Dispatch();
        }
    }

    private void OnDataReceivedError(PlayFabError error)
    {
        Debug.Log("Error while getting data to server");
        Debug.Log(error.GenerateErrorReport());
    }

    public void Login(Action callback)
    {
        OnLoginSucessCallback = callback;
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
        OnLoginSucessCallback();
        OnLoginSucessCallback = null;
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error while login in/creating account!");
        Debug.Log(error.GenerateErrorReport());
        OnLoginSucessCallback = null;
    }
}
