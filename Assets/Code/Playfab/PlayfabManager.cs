using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;

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
    
    public void Login(Action callback)
    {
        OnLoginSucessCallback = callback;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnLoginError);
    }

    private void OnSuccess(LoginResult result)
    {
        Debug.Log("Succesful login/account create!");
        OnLoginSucessCallback();
        OnLoginSucessCallback = null;
    }

    private void OnLoginError(PlayFabError error)
    {
        Debug.Log("Error while login in/creating account!");
        Debug.Log(error.GenerateErrorReport());
        OnLoginSucessCallback = null;
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
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
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
    
    private void OnError(PlayFabError error)
    {
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

    public void GetUserInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        InventoryUtility.UpdateInventory(result.VirtualCurrency);
    }

    public void AddCurrency(string type, int value)
    {
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest()
        {
            VirtualCurrency = type,
            Amount = value
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request,OnAddCurrencySuccess, OnError);
    }

    private void OnAddCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"Added currency!");
        InventoryUtility.UpdateItem(result.VirtualCurrency, result.Balance);
    }

    public void SubtractCurrency(string type, int value)
    {
        SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest()
        {
            VirtualCurrency = type,
            Amount = value
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractCurrencySuccess, OnError);
    }

    private void OnSubtractCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log($"Subtracted currency!");
        InventoryUtility.UpdateItem(result.VirtualCurrency, result.Balance);
    }

    // TODO: Can we use Cloudscript for this method?
    public void BuyItem(string type, int value, string costCurrency, int cost)
    {
        ExecuteCloudScriptRequest buyRequest = new ExecuteCloudScriptRequest
        {
            FunctionName = "BuyItem",
            FunctionParameter = new
            {
                itemToBuy = type,
                itemAmount = value,
                itemCostCurrency = costCurrency,
                itemCost = cost
            }
        };
        
        PlayFabClientAPI.ExecuteCloudScript(buyRequest, result =>
        {
            Debug.Log(result.FunctionResult);
        }, error =>
        {
            Debug.Log("ExecuteCloudScript There was an error");
        });
    }

    private void OnBuySuccess(ModifyUserVirtualCurrencyResult result, string currencyToSubstract, int cost)
    {
        InventoryUtility.UpdateItem(result.VirtualCurrency, result.Balance);
        SubtractCurrency(currencyToSubstract, cost);
    }

    private void GetCurrencyRemainingTime()
    {
        // TODO: Create a method in cloudscript to return remaining time.
    }
}
