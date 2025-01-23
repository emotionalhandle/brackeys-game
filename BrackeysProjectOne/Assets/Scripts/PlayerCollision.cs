using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayMovement movement;
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
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
                FindObjectOfType<GameManager>().EndGame();
            }
        }


        Debug.Log(collisionInfo.collider.name);
    }
}
