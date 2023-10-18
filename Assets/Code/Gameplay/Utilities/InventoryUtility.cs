using System.Collections.Generic;

public static class InventoryUtility
{
    // Economy
    public static Dictionary<string, int> Inventory = new Dictionary<string, int>();

    // TODO: Find a better way to handle this
    public static Dictionary<string, CurrencyInfo> currencyData = new Dictionary<string, CurrencyInfo>();

    public static void UpdateInventory(Dictionary<string, int> values)
    {
        Inventory = new Dictionary<string, int>();
        foreach (var value in values)
        {
            Inventory.Add(value.Key, value.Value);
        }
        Events.OnInventoryDataReceived.Dispatch();
    }

    public static void UpdateItem(string key, int value)
    {
        Inventory[key] = value;
        Events.OnInventoryDataReceived.Dispatch();
    }

    public static int Coins => Inventory[GameConstants.Currencies.COINS];
    public static int Diamonds => Inventory[GameConstants.Currencies.DIAMONDS];
    public static bool Nitro => Inventory[GameConstants.Currencies.NITRO] > 0;
}