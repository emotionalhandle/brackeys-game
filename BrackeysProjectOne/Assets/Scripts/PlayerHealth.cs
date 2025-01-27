using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerMovement movement;
    public TextMeshProUGUI healthText;
    private int health;

    void Start()
    {
        health = GameData.PlayerHealth;
        healthText.text = $"Health: {health}";
    }

    public void DamagePlayer()
    {
        health -= 1; 
        GameData.PlayerHealth = health;
        healthText.text = $"Health: {health}";
        if (health == 0)
        {
            movement.enabled = false;
            FindAnyObjectByType<GameManager>().RestartLevel();
        }
    }
}
