public static class GameData
{
    private static int playerHealth = 3; // Default starting health
    private static float remainingTime = 0f;

    public static int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public static float RemainingTime
    {
        get { return remainingTime; }
        set { remainingTime = value; }
    }

    public static void ResetHealth()
    {
        playerHealth = 3;
    }

    public static void ResetTime()
    {
        remainingTime = 0f;
    }
}