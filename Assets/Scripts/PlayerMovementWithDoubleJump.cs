using UnityEngine;

public class PlayerMovementWithDoubleJump : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;
    [SerializeField] float jumpForce = 10.0f;
    [SerializeField] Transform cameraMovement;
    Rigidbody rb;

    Vector2 playerInput;
    float jump;
    bool grounded;
    bool jumpCheck = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        jumpCheck = jumpCheck || Input.GetButtonDown("Jump");
    }
    void FixedUpdate()
    {
        readInput();
        writeInputs();
        if (jumpCheck && grounded)
        {
            OtherJump();
            jumpCheck = false;
        }
        grounded = false;
    }
    void readInput()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1);
    }
    void writeInputs()
    {
        Vector3 cameraForward = cameraMovement.forward;
        Vector3 cameraRight = cameraMovement.right;
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementTotal = cameraForward * playerInput.y + cameraRight * playerInput.x;
        movementTotal.Normalize();

        Vector3 v = rb.linearVelocity;
        v.x = movementTotal.x * speed;
        v.z = movementTotal.z * speed;
        rb.linearVelocity = v;
    }
    void OtherJump()
    {
        jump = Input.GetAxisRaw("Jump");

        Vector3 v = rb.linearVelocity;
        v.y = jump * jumpForce;      
        rb.linearVelocity = v;
    }
    void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
}
