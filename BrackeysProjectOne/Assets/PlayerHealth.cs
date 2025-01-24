using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerMovement movement;
    public int lives = 4;
    public TextMeshProUGUI health;

    void Start()
    {
        health.text = $"Health: {lives}";
    }

    public void DamagePlayer()
    {
        lives -= 1; 
        health.text = $"Health: {lives}";
        if (lives == 0)
        {
            movement.enabled = false;
            FindAnyObjectByType<GameManager>().EndGame();
        }
    }
}
