using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class BasicFirstPersonController : MonoBehaviour
{
    [Header("View Reference")]
    public Transform viewTransform;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.12f;
    public LayerMask groundLayers = ~0;

    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool jumpRequested;

    void Reset()
    {
        if (viewTransform == null && Camera.main != null)
        {
            viewTransform = Camera.main.transform;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (jumpRequested)
        {
            Jump();
            jumpRequested = false;
        }
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        if (viewTransform != null)
        {
            forward = viewTransform.forward;
            right = viewTransform.right;
        }

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        Vector3 targetVelocity = moveDirection * moveSpeed;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;
    }

    void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    bool IsGrounded()
    {
        Vector3 center = capsule.bounds.center;
        float radius = capsule.radius * 0.95f;
        float castDistance = (capsule.bounds.extents.y - radius) + groundCheckDistance;

        return Physics.SphereCast(
            center,
            radius,
            Vector3.down,
            out RaycastHit _,
            castDistance,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );
    }
}
