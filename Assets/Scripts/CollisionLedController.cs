using UnityEngine;

// This component compares two assigned colliders and mirrors their collision
// state to Arduino by controlling the red LED output command.
public class CollisionLedController : MonoBehaviour
{
    [Header("Data Source")]
    public ArduinoDataInputOutput dataIO;

    [Header("Collision Targets")]
    public Collider firstTarget;
    public Collider secondTarget;

    private bool previousCollisionState;
    private bool missingTargetErrorShown;

    void Reset()
    {
        dataIO = FindFirstObjectByType<ArduinoDataInputOutput>();
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
            previousCollisionState = isColliding;
            SendCollisionState(isColliding);
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
}
