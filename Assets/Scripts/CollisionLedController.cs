using UnityEngine;

// This component compares two assigned colliders and mirrors their collision
// state to Arduino by controlling the red LED output command.
public class CollisionLedController : MonoBehaviour
{
    [Header("Data Source")]
    public ArduinoDataInputOutput dataIO;

    [Header("Collision Audio")]
    public AudioSource audioSource;
    public AudioClip collisionSfx;
    [Range(0f, 1f)]
    public float collisionSfxVolume = 1f;

    [Header("Collision Targets")]
    public Collider firstTarget;
    public Collider secondTarget;

    private bool previousCollisionState;
    private bool missingTargetErrorShown;

    void Reset()
    {
        dataIO = FindFirstObjectByType<ArduinoDataInputOutput>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        bool isColliding = CheckCollision();
        previousCollisionState = isColliding;
        SendCollisionState(isColliding);

        // Same startup reliability pattern used elsewhere in this lesson.
        Invoke(nameof(ResendInitialCollisionState), 0.75f);
    }

    void Update()
    {
        bool isColliding = CheckCollision();

        if (isColliding != previousCollisionState)
        {
            bool collisionEntered = isColliding && !previousCollisionState;
            previousCollisionState = isColliding;
            SendCollisionState(isColliding);

            if (collisionEntered)
            {
                PlayCollisionSound();
            }
        }
    }

    bool CheckCollision()
    {
        if (firstTarget == null || secondTarget == null)
        {
            if (!missingTargetErrorShown)
            {
                Debug.LogError("CollisionLedController requires both collision targets to be assigned in the Inspector.", this);
                missingTargetErrorShown = true;
            }

            return false;
        }

        return firstTarget.bounds.Intersects(secondTarget.bounds);
    }

    void SendCollisionState(bool isColliding)
    {
        if (dataIO != null)
        {
            dataIO.SendRedLedState(isColliding);
        }
    }

    void ResendInitialCollisionState()
    {
        SendCollisionState(previousCollisionState);
    }

    void PlayCollisionSound()
    {
        if (collisionSfx == null)
        {
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(collisionSfx, collisionSfxVolume);
            return;
        }

        AudioSource.PlayClipAtPoint(collisionSfx, transform.position, collisionSfxVolume);
    }
}
