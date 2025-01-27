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

    [Header("Lane System")]
    public float laneDistance = 12f;  // Increased from 4f to 12f for wider lanes
    public float laneSwitchSpeed = 10f;  // How fast to switch lanes
    public int currentLane = 2;  // Start in middle lane (0-4 for 5 lanes)
    private float targetX;  // Target X position for lane
    
    [Header("Speed System")]
    public float baseForwardSpeed = 2000f;
    public float speedMultiplier = 1f;
    private float maxSpeedMultiplier = 2f;
    public float speedIncreaseRate = 0.1f;

    void Start()
    {
        remainingJumps = maxJumps;
        targetX = transform.position.x;
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
        // Forward movement with increasing speed
        float currentSpeed = forwardForce * speedMultiplier * Time.deltaTime;
        rb.AddForce(0, 0, currentSpeed);
        
        // Gradually increase speed
        if (speedMultiplier < maxSpeedMultiplier)
        {
            speedMultiplier += speedIncreaseRate * Time.deltaTime;
        }

        // Lane switching with A/D keys
        if (Input.GetKeyDown("d") && currentLane < 4)
        {
            currentLane++;
            targetX = currentLane * laneDistance - (2 * laneDistance); // Center middle lane at x=0
        }

        if (Input.GetKeyDown("a") && currentLane > 0)
        {
            currentLane--;
            targetX = currentLane * laneDistance - (2 * laneDistance);
        }

        // Smooth lane movement
        float currentX = transform.position.x;
        if (Mathf.Abs(currentX - targetX) > 0.1f)
        {
            float newX = Mathf.Lerp(currentX, targetX, laneSwitchSpeed * Time.deltaTime);
            rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z));
        }

        // Keep existing jump and gravity mechanics
        rb.AddForce(0, -downwardForce * rb.mass, 0);

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
