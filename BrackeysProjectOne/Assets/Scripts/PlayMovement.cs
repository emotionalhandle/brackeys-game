using UnityEngine;

public class PlayMovement : MonoBehaviour
{
    private bool isGrounded;
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float jumpForce = 20f;
    public float downwardForce = 50f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground") 
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(0,0,forwardForce * Time.deltaTime);
        rb.AddForce(0, -downwardForce * rb.mass, 0); // Add additional downward force to simulate increased gravity
    

        if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime,0,0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime,0,0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("space") && isGrounded)
        {
            rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
        }

        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
