using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerHealth health;

    // Define the HitDirection enum
    private enum HitDirection
    {
        Top,
        Front,
        Back,
        Right,
        Left,
        Unknown
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            HitDirection hitDirection = DetermineHitDirection(collisionInfo);
            Debug.Log(hitDirection);
            movement.KnockBack();
            health.DamagePlayer();
            //movement.enabled = false;
            //FindAnyObjectByType<GameManager>().EndGame();
        }

        if (collisionInfo.collider.tag == "DestructibleObstacle")
        {
            HitDirection hitDirection = DetermineHitDirection(collisionInfo);
            Debug.Log(hitDirection);
            if (hitDirection == HitDirection.Top)
            {
                Debug.Log("Should delete");
                movement.remainingJumps = movement.maxJumps;
                StartCoroutine(HandleDestruction(collisionInfo.collider.GetComponent<DestructibleObstacle>()));
            }
            else
            {
                movement.enabled = false;
                FindAnyObjectByType<GameManager>().RestartLevel();
            }
        }

        Debug.Log(collisionInfo.collider.name);
    }

    private IEnumerator HandleDestruction(DestructibleObstacle obs)
    {
        Renderer renderer = obs.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color originalColor = renderer.material.color;
            Color destructionColor = Color.red; // Change to red or any color you prefer
            renderer.material.color = destructionColor;
        }
        // Wait for a short duration before destroying the obstacle
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed

        if (obs != null)
        {
            obs.DestroyObstacle();
        }
    }

    private HitDirection DetermineHitDirection(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Vector3 hitNormal = contact.normal;

            if (hitNormal.y > 0.5f) 
            {
                return HitDirection.Top;
            }
            else if (hitNormal.z < -0.5f) 
            {
                return HitDirection.Front;
            }
            else if (hitNormal.z > 0.5f) 
            {
                return HitDirection.Back;
            }
            else if (hitNormal.x > 0.5f) 
            {
                return HitDirection.Right;
            }
            else if (hitNormal.x < -0.5f) 
            {
                return HitDirection.Left;
            }
        }
        return HitDirection.Unknown; // Default case
    }
}
