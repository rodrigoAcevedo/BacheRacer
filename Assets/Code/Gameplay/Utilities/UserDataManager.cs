public static class UserDataManager
{
    public static string PlayerHealth { get; private set; }

    public static void SetPlayerHealth(string value)
    {
        PlayerHealth = value;
    }
}
