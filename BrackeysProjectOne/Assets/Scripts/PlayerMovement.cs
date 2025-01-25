using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float jumpForce = 20f;
    public float downwardForce = 50f;

    private int maxJumps = 2;  // Maximum number of jumps allowed
    public int remainingJumps;  // Current number of jumps available

    void Start()
    {
        remainingJumps = maxJumps;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground") 
        {
            isGrounded = true;
            remainingJumps = maxJumps;  // Reset jumps when touching ground
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    public void KnockBack()
    {
        rb.AddForce(0,0, -forwardForce * Time.deltaTime, ForceMode.VelocityChange);
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

        if (Input.GetKeyDown("space") && remainingJumps > 0)
        {
            rb.AddForce(0, jumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
            remainingJumps--;
        }

        if (rb.position.y < -1f)
        {
            FindAnyObjectByType<GameManager>().EndGame();
        }
    }
}
