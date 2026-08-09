using UnityEngine;

// This component watches one assigned collider and turns the Arduino red LED
// on whenever it collides/overlaps with any other valid collider.
public class AnyCollisionLedController : MonoBehaviour
{
    [Header("Data Source")]
    public ArduinoDataInputOutput dataIO;

    [Header("Collision Target")]
    public Collider monitoredCollider;

    [Header("Collision Filters")]
    public LayerMask validLayers = ~0;
    public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;

    [Header("Output Pulse")]
    public int redLedPulseDurationMs = 250;

    private bool previousCollisionState;
    private bool missingTargetErrorShown;

    void Reset()
    {
        dataIO = FindFirstObjectByType<ArduinoDataInputOutput>();
    }

    void Start()
    {
        previousCollisionState = CheckCollisionWithAnyValidObject();
    }

    void Update()
    {
        bool isColliding = CheckCollisionWithAnyValidObject();

        // Entry-only behavior: send pulse when collision first begins.
        if (isColliding && !previousCollisionState)
        {
            SendCollisionPulse();
        }

        previousCollisionState = isColliding;
    }

    bool CheckCollisionWithAnyValidObject()
    {
        if (monitoredCollider == null)
        {
            if (!missingTargetErrorShown)
            {
                Debug.LogError("AnyCollisionLedController requires monitoredCollider to be assigned in the Inspector.", this);
                missingTargetErrorShown = true;
            }

            return false;
        }

        Bounds bounds = monitoredCollider.bounds;

        Collider[] hits = Physics.OverlapBox(
            bounds.center,
            bounds.extents,
            monitoredCollider.transform.rotation,
            validLayers,
            triggerInteraction
        );

        for (int i = 0; i < hits.Length; i++)
        {
            Collider other = hits[i];

            if (other == null)
            {
                continue;
            }

            if (other == monitoredCollider)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    void SendCollisionPulse()
    {
        if (dataIO != null)
        {
            int duration = Mathf.Max(1, redLedPulseDurationMs);
            dataIO.SendRedLedPulse(duration);
        }
    }
}
