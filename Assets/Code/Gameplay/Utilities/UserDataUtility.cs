using System.Collections.Generic;
using PlayFab.ClientModels;

public static class UserDataUtility
{
    public static Dictionary<string, UserDataRecord> Data = new Dictionary<string, UserDataRecord>();

    public static void GetUserDataFromServer()
    {
        PlayfabManager.Instance.GetData();
    }
    
    public static void SetPlayerHealth(float value)
    {
        PlayfabManager.Instance.SaveData(GameConstants.PlayfabProperties.HEALTH, value.ToString());
        SaveOnLocalData(GameConstants.PlayfabProperties.HEALTH, value.ToString());
    }

    public static void RestorePlayerHealth()
    {
        // TODO: This could be better managed with a Store class utility.
        if (GameManager.Instance.LastKnownPlayerHealth <= 0 && InventoryUtility.Coins >= 10)
        {
            SetPlayerHealth(GameManager.Instance.BaseParameters.DefaultPlayerHealth);
            PlayfabManager.Instance.SubtractCurrency(GameConstants.Currencies.COINS, 10);
            // Also we could make a method to tie this directly to the success callback of the substract currency.
            Events.OnDataReceived.Dispatch();
        }
    }

    public static float GetPlayerHealth()
    {
        if (Data.ContainsKey(GameConstants.PlayfabProperties.HEALTH))
        {
            return float.Parse(Data[GameConstants.PlayfabProperties.HEALTH].Value);
        }

        return GameManager.Instance.BaseParameters.DefaultPlayerHealth;
    }

    public static void SetPlayerKilometersRan(float value)
    {
        PlayfabManager.Instance.SaveData(GameConstants.PlayfabProperties.KILOMETERS_RAN, value.ToString());
        SaveOnLocalData(GameConstants.PlayfabProperties.KILOMETERS_RAN, value.ToString());
    }

    public static int GetPlayerKilometersRan()
    {
        if (Data.ContainsKey(GameConstants.PlayfabProperties.KILOMETERS_RAN))
        {
            return int.Parse(Data[GameConstants.PlayfabProperties.KILOMETERS_RAN].Value);
        }

        return GameManager.Instance.BaseParameters.DefaultKilometersRan;
    }

    private static void SaveOnLocalData(string key, string value)
    {
        if (!Data.ContainsKey(key))
        {
            UserDataRecord userDataRecord = new UserDataRecord()
            {
                Value = value
            };
            Data.Add(key, userDataRecord);
        }
        else
        {
            Data[key].Value = value;
        }
    }
}
