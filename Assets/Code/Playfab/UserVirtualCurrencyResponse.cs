using System;
using System.Collections.Generic;

[System.Serializable]
public class UserVirtualCurrencyResponse
{
    public string PlayFabId;
    public string VirtualCurrency;
    public int BalanceChange;
    public int Balance;
}

[System.Serializable]
public class BuyResponse
{
    public UserVirtualCurrencyResponse subtractedCurrency;
    public UserVirtualCurrencyResponse addedCurrency;
}

// Testing purposes only
public class CurrencyInfo
{
    public int SecondsToRecharge { get; set; }
    public DateTime RechargeTime { get; set; }
    public int RechargeMax { get; set; }
}

public class RootObject
{
    public Dictionary<string, CurrencyInfo> Currencies { get; set; }
}
