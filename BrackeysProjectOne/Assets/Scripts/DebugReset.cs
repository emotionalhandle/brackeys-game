using UnityEngine;
using UnityEngine.UI;

public class DebugReset : MonoBehaviour
{
    private Button resetButton;

    void Start()
    {
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(ResetPlayer);
    }

    public void ResetPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Reset position
            player.transform.position = new Vector3(0, 2, 0);
            
            // Reset velocity
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Reset lane position
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.currentLane = 2; // Reset to middle lane
            }
        }
    }
} 