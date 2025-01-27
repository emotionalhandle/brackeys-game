using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        TimeUp,
        Failed,
        Completed
    }

    private GameState currentState = GameState.Playing;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    public GameObject levelFailedUI;

    // Cache references
    private GameObject player;
    private PlayerMovement playerMovement;
    private PlayerCollision playerCollision;
    private Collider playerCollider;
    private Rigidbody playerRb;

    void Start()
    {
        GameData.ResetHealth();
        
        // Cache all player references
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCollision = player.GetComponent<PlayerCollision>();
            playerCollider = player.GetComponent<Collider>();
            playerRb = player.GetComponent<Rigidbody>();
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        Debug.Log("Level complete");
    }

    public void TimeUp()
    {
        // Cancel any pending restarts and allow TimeUp even if Failed state was just set
        CancelInvoke();
        if (currentState == GameState.Playing || currentState == GameState.Failed)
        {
            currentState = GameState.TimeUp;
            levelFailedUI.SetActive(true);
            
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                playerRb.isKinematic = true;
            }
            
            if (playerMovement != null) playerMovement.enabled = false;
            if (playerCollision != null) playerCollision.enabled = false;
            if (playerCollider != null) playerCollider.enabled = false;
            
            Debug.Log("Time's up!");
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting level!");
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Failed;
            Debug.Log("GAME OVER - Restarting Level");
            Invoke("LoadCurrentScene", restartDelay);
        }
    }

    private void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
