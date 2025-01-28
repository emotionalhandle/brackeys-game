using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGrounded;
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float jumpForce = 20f;
    public float downwardForce = 50f;

    public int maxJumps = 2;  // Maximum number of jumps allowed
    public int remainingJumps;  // Current number of jumps available

    [Header("Lane System")]
    public int numberOfLanes = 5;  // Adjustable number of lanes in the editor
    public float laneDistance = 8f;  // Increased from 4f to 8f for wider lanes
    public float laneSwitchSpeed = 10f;  // How fast to switch lanes
    public int currentLane = 2;  // Start in middle lane (0 to numberOfLanes-1)
    private float targetX;  // Target X position for lane
    
    [Header("Speed System")]
    public float baseForwardSpeed = 2000f;
    public float speedMultiplier = 1f;

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

    void Update()
    {
        // Lane switching with A/D keys - detect input in Update for responsiveness
        if (Input.GetKeyDown(KeyCode.D) && currentLane < numberOfLanes - 1)
        {
            currentLane++;
            targetX = (currentLane - (numberOfLanes / 2)) * laneDistance;  // Adjusted formula
        }

        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
            targetX = (currentLane - (numberOfLanes / 2)) * laneDistance;  // Adjusted formula
        }

        // Jump input detection
        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            remainingJumps--;
        }
    }

    void FixedUpdate()
    {
        // Forward movement with increasing speed
        float currentSpeed = forwardForce * speedMultiplier * Time.deltaTime;
        rb.AddForce(0, 0, currentSpeed);

        // Smooth lane movement
        float currentX = transform.position.x;
        if (Mathf.Abs(currentX - targetX) > 0.1f)
        {
            float newX = Mathf.Lerp(currentX, targetX, laneSwitchSpeed * Time.deltaTime);
            rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z));
        }
        
        // Snap to the target lane position if close enough
        if (Mathf.Abs(currentX - targetX) <= 0.1f)
        {
            rb.MovePosition(new Vector3(targetX, rb.position.y, rb.position.z));  // Snap to targetX
        }

        // Keep existing gravity mechanics
        rb.AddForce(0, -downwardForce * rb.mass, 0);

        if (rb.position.y < -1f)
        {
            FindAnyObjectByType<GameManager>().RestartLevel();
        }
    }
}
