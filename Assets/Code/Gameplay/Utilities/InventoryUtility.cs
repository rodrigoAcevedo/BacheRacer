using System.Collections.Generic;

public static class InventoryUtility
{
    // Economy
    private static Dictionary<string, int> Inventory = new Dictionary<string, int>();

    public static void UpdateInventory(Dictionary<string, int> values)
    {
        Inventory = new Dictionary<string, int>();
        foreach (var value in values)
        {
            Inventory.Add(value.Key, value.Value);
        }
    }

    public static void UpdateItem(string key, int value)
    {
        Inventory[key] = value;
    }

    public static int Coins => Inventory["CN"];
    public static int Diamonds => Inventory["DM"];
    public static bool Nitro => Inventory["NO"] > 0;
}