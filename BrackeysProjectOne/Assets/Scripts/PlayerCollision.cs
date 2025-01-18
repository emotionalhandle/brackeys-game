using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayMovement movement;
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
        }
        Debug.Log(collisionInfo.collider.name);
    }
}
