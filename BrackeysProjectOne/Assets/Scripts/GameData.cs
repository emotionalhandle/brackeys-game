public static class GameData
{
    private static int playerHealth = 4; // Default starting health
    private static int startHealth = 4;

    public static int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public static void ResetHealth()
    {
        playerHealth = startHealth;
    }
}