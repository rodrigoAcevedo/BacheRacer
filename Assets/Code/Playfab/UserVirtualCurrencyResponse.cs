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