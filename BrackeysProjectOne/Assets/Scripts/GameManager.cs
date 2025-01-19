using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameOver = false;

    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        Debug.Log("Level complete");
    }
    public void EndGame()
    {
        if (gameOver == false)
        {
            gameOver = true;
            Debug.Log("GAME OVER");
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
