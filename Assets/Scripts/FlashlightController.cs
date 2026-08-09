using UnityEngine;

// This component reads a potentiometer value from ArduinoDataInputOutput
// and maps that value to a Z-axis rotation for a chosen target object.
public class FlashlightController : MonoBehaviour
{
    [Header("Data Source")]
    // Reference to the script that receives serial data from Arduino.
    public ArduinoDataInputOutput dataIO;

    [Header("Target")]
    // The object this script controls. Useful when this script is on an empty GameObject.
    public Transform targetObject;
    // Light to toggle with button input. Must be assigned in the Inspector.
    public Light targetLight;

    [Header("Potentiometer Input Range")]
    // Raw sensor range expected from Arduino (analogRead is usually 0-1023).
    public int minPotValue = 0;
    public int maxPotValue = 1023;

    [Header("Cylinder Z Rotation (Degrees)")]
    // Rotation range to map onto. Example: 0 to 180 for half a turn.
    public float minZAngle = 0f;
    public float maxZAngle = 360f;
    // Flip the direction if turning the knob feels backwards.
    public bool invertDirection = false;
    // Local rotation uses the target's parent as reference; world rotation uses scene axes.
    public bool useLocalRotation = true;

    [Header("Optional Smoothing")]
    [Tooltip("Set to 0 for no smoothing. Higher values respond faster.")]
    public float smoothingSpeed = 0f;

    // Tracks the angle we are currently applying (used for smoothing).
    private float currentZAngle;
    // Stores the target's original X/Y so we only control Z.
    private Vector3 baseEulerAngles;
    // Used to detect if a different target was assigned at runtime.
    private Transform cachedTarget;
    // Tracks button transitions so one press toggles once.
    private bool previousButtonPressed;
    // Prevents repeating the same missing-assignment error every frame.
    private bool missingLightErrorShown;

    // Called when the component is first added or reset in the Inspector.
    void Reset()
    {
        dataIO = FindFirstObjectByType<ArduinoDataInputOutput>();
        targetObject = transform;
    }

    // Start runs once before the first Update.
    void Start()
    {
        EnsureTarget();
        CacheBaseRotation();
        currentZAngle = GetTargetZAngle();

        if (targetLight == null)
        {
            Debug.LogError("FlashlightController requires targetLight to be assigned in the Inspector.", this);
            missingLightErrorShown = true;
        }
        else
        {
            // Send startup state now, then resend once after a short delay.
            // The delayed send helps when Arduino briefly resets after serial opens.
            SendFlashlightStateToArduino();
            Invoke(nameof(SendInitialFlashlightStateDelayed), 0.75f);
        }

        previousButtonPressed = dataIO != null && dataIO.buttonPressed;
    }

    // Update runs once per frame.
    void Update()
    {
        // If we do not have sensor input, do nothing this frame.
        if (dataIO == null)
        {
            return;
        }

        EnsureTarget();

        // No valid target to control.
        if (targetObject == null)
        {
            return;
        }

        // If the target changes, refresh baseline rotation data.
        if (targetObject != cachedTarget)
        {
            CacheBaseRotation();
            currentZAngle = GetTargetZAngle();
        }

        // Toggle the light only on the button's rising edge (not every frame while held).
        bool currentButtonPressed = dataIO.buttonPressed;

        if (currentButtonPressed && !previousButtonPressed)
        {
            ToggleLight();
        }

        previousButtonPressed = currentButtonPressed;

        // Convert sensor value into a normalized 0..1 value.
        float t = Mathf.InverseLerp(minPotValue, maxPotValue, dataIO.potValue);

        if (invertDirection)
        {
            t = 1f - t;
        }

        // Map normalized value to the desired angle range.
        float targetZAngle = Mathf.Lerp(minZAngle, maxZAngle, t);

        if (smoothingSpeed > 0f)
        {
            // Exponential smoothing gives a responsive but less jittery movement.
            float blend = 1f - Mathf.Exp(-smoothingSpeed * Time.deltaTime);
            currentZAngle = Mathf.LerpAngle(currentZAngle, targetZAngle, blend);
        }
        else
        {
            // No smoothing: jump directly to the mapped angle.
            currentZAngle = targetZAngle;
        }

        ApplyZRotation(currentZAngle);
    }

    // If no target is assigned, default to the object this script is attached to.
    void EnsureTarget()
    {
        if (targetObject == null)
        {
            targetObject = transform;
        }
    }

    // Save the current rotation as a baseline so X/Y stay unchanged while Z is controlled.
    void CacheBaseRotation()
    {
        cachedTarget = targetObject;

        if (cachedTarget == null)
        {
            return;
        }

        baseEulerAngles = useLocalRotation ? cachedTarget.localEulerAngles : cachedTarget.eulerAngles;
    }

    // Reads the target's current Z angle.
    float GetTargetZAngle()
    {
        if (targetObject == null)
        {
            return 0f;
        }

        Vector3 euler = useLocalRotation ? targetObject.localEulerAngles : targetObject.eulerAngles;
        return euler.z;
    }

    void ApplyZRotation(float zAngle)
    {
        // Apply an absolute Z angle while preserving baseline X/Y.
        Vector3 euler = baseEulerAngles;
        euler.z = zAngle;

        if (useLocalRotation)
        {
            targetObject.localEulerAngles = euler;
        }
        else
        {
            targetObject.eulerAngles = euler;
        }
    }

    // Switches the light between on/off states.
    void ToggleLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
            SendFlashlightStateToArduino();
        }
        else if (!missingLightErrorShown)
        {
            Debug.LogError("FlashlightController requires targetLight to be assigned in the Inspector.", this);
            missingLightErrorShown = true;
        }
    }

    void SendFlashlightStateToArduino()
    {
        if (dataIO != null && targetLight != null)
        {
            dataIO.SendFlashlightState(targetLight.enabled);
        }
    }

    // Sends one delayed startup sync to improve reliability after serial startup.
    void SendInitialFlashlightStateDelayed()
    {
        if (dataIO == null || targetLight == null)
        {
            return;
        }

        SendFlashlightStateToArduino();
    }
}
