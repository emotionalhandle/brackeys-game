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

    private float resetDuration = 0.5f; // Duration for the reset effect
    private float resetTimer = 0f; // Timer for the reset effect
    private Quaternion initialRotation; // Store the initial rotation

    public float baseSpinForce = 1f; // Base amount of force to apply for spinning
    public float maxSpinForce = 50f; // Maximum spin force
    public float chargeRate = 5f; // Rate at which spin force is charged

    private float currentSpinForce = 0f; // Current spin force being applied

    void Start()
    {
        remainingJumps = maxJumps;
        targetX = transform.position.x;
        initialRotation = transform.rotation; // Store the initial rotation
    }

    public void ResetOrientation()
    {
        resetTimer = 0f; // Reset the timer
        initialRotation = transform.rotation; // Store the current rotation as the initial rotation
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground") 
        {
            isGrounded = true;
            remainingJumps = maxJumps;  // Reset jumps when touching ground
        }
        else if (collision.collider.tag == "Obstacle")
        {
            // Call the reset orientation method after hitting an obstacle
            ResetOrientation();
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
        // Gradually reset orientation if the timer is active
        if (resetTimer < resetDuration)
        {
            resetTimer += Time.deltaTime;
            float t = resetTimer / resetDuration; // Calculate the interpolation factor
            transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.identity, t); // Lerp to the target rotation

            // Gradually reduce angular velocity
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, t);
        }

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

        // Charge spin while holding down the S key
        if (Input.GetKey(KeyCode.S))
        {
            // Increase the current spin force based on the charge rate
            currentSpinForce += chargeRate * Time.deltaTime; // Accumulate spin force
            currentSpinForce = Mathf.Clamp(currentSpinForce, 0, maxSpinForce); // Clamp to max spin force

            // Apply the current spin force as torque
            rb.AddTorque(Vector3.up * currentSpinForce * Time.deltaTime, ForceMode.VelocityChange); // Apply torque for spinning
        }
        else
        {
            // Reset the spin force when the key is released
            currentSpinForce = 0f; // Reset the spin force when the key is not held
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
