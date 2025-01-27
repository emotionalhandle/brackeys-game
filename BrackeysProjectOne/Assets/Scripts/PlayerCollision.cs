using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerHealth health;
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.KnockBack();
            health.DamagePlayer();
            //movement.enabled = false;
            //FindAnyObjectByType<GameManager>().EndGame();
        }

        if (collisionInfo.collider.tag == "DestructibleObstacle")
        {
            float maxY = float.MinValue; 
            foreach (ContactPoint contact in collisionInfo.contacts)
            {
                if (contact.point.y > maxY)
                {
                    maxY = contact.point.y; 
                }
            }
            if (transform.position.y > maxY)
            {
                DestructibleObstacle obs = collisionInfo.collider.GetComponent<DestructibleObstacle>();
                if (obs != null)
                {
                    obs.DestroyObstacle();
                } 
                Debug.Log("Landed on top");
            }
            else
            {
                movement.enabled = false;
                FindAnyObjectByType<GameManager>().RestartLevel();
            }
        }


        Debug.Log(collisionInfo.collider.name);
    }
}
