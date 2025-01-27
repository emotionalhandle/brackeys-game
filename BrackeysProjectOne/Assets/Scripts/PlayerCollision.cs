using Unity.VisualScripting;
using UnityEngine;

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
                DestructibleObstacle obs = collisionInfo.collider.GetComponent<DestructibleObstacle>();
                if (obs != null)
                {
                    obs.DestroyObstacle();
                } 
            }
            else
            {
                movement.enabled = false;
                FindAnyObjectByType<GameManager>().RestartLevel();
            }
        }

        Debug.Log(collisionInfo.collider.name);
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
