using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;
    public void EndGame()
    {
        if (gameOver == false)
        {
            gameOver = true;
            Debug.Log("GAME OVER");
        }
    }
}
