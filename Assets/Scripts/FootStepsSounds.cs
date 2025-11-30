using UnityEngine;

public class FootStepsSounds : MonoBehaviour
{
    public AudioClip footstepClip;
    public float stepInterval = 0.45f;
    public float minSpeedToStep = 0.1f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask = ~0;

    Rigidbody rb;
    AudioSource src;
    float timer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 1f;
    }

    void FixedUpdate()
    {
        Vector3 horiz = rb.linearVelocity;
        horiz.y = 0f;
        float speed = horiz.magnitude;

        bool grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        if (speed > minSpeedToStep && grounded)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= stepInterval)
            {
                if (footstepClip != null) src.PlayOneShot(footstepClip);
                timer = 0f;
            }
        }
        else timer = 0f;
    }
}
