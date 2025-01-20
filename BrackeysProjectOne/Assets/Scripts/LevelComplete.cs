using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
