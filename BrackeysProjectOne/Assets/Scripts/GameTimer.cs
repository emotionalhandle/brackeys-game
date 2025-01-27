using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float gameDuration = 300f; // 5 minutes in seconds
    private float currentTime;
    private bool isGameOver = false;

    void Start()
    {
        // If there's no saved time, start fresh
        if (GameData.RemainingTime <= 0)
        {
            currentTime = gameDuration;
            GameData.RemainingTime = currentTime;
        }
        else
        {
            currentTime = GameData.RemainingTime;
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;
            GameData.RemainingTime = currentTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isGameOver = true;
                FindAnyObjectByType<GameManager>().EndGame();
            }

            DisplayTime();
        }
    }

    void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
} 