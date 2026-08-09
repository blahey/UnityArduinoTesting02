# UnityArduinoTesting02 Project Knowledge Base

## 1. Purpose and scope
This project demonstrates a bidirectional Unity-Arduino interaction pipeline for digital-physical game experiences.

It currently includes:
- Sensor input from Arduino into Unity.
- Unity scene logic driven by sensor data.
- Unity output commands back to Arduino for physical feedback.
- Progressive interaction patterns suitable for lesson-based teaching.

## 2. Core architecture
The system is organized into two runtime layers:

1. Unity runtime layer
- Reads serial sensor data through ArduinoDataInputOutput.
- Applies sensor-driven gameplay behavior through specialized controller scripts.
- Sends command messages to Arduino for actuator output.

2. Arduino firmware layer
- Streams sensor updates to Unity in CSV format.
- Receives Unity command messages.
- Drives actuator pins based on Unity state and Unity events.

## 3. Current hardware mapping
- Potentiometer input pin: A0
- Button input pin: 2 (active low, INPUT_PULLUP)
- Green LED output pin: 7
- Red LED output pin: 8

## 4. Serial protocol reference
Baud rate:
- 115200

Direction: Arduino -> Unity
- Message format: potValue,buttonState
- Example: 512,1
- Frequency: every 50 ms (SEND_INTERVAL_MS)

Direction: Unity -> Arduino
- FLASHLIGHT,0 or FLASHLIGHT,1
- REDPULSE,durationMs
- REDLED,1 (legacy compatibility path, triggers default red pulse in firmware)

## 5. Script inventory and responsibilities

### ArduinoDataInputOutput
File: Assets/Scripts/ArduinoDataInputOutput.cs

Responsibilities:
- Opens and manages serial port lifecycle.
- Parses incoming Arduino CSV lines with safe parsing.
- Exposes potValue and buttonPressed for other Unity scripts.
- Sends outbound command messages:
  - SendFlashlightState(bool)
  - SendRedLedState(bool)
  - SendRedLedPulse(int)

Design notes:
- Uses read/write timeouts to avoid lockups.
- Uses TryParse and trimming for robust runtime parsing.
- Optional outgoing message logging via logOutgoingMessages.

### FlashlightController
File: Assets/Scripts/FlashlightController.cs

Responsibilities:
- Maps potentiometer range to absolute Z rotation for a selected target object.
- Toggles selected Unity Light on button rising edge.
- Sends current Unity light state to Arduino green LED.

Key behavior decisions:
- Target assignment is explicit for teaching clarity.
- Light assignment is explicit for teaching clarity.
- Startup state sync uses immediate send plus delayed resend for serial-reset resilience.

### CollisionLedController
File: Assets/Scripts/CollisionLedController.cs

Responsibilities:
- Detects overlap state between two explicitly assigned colliders.
- Mirrors state to Arduino red output command path.

Use case:
- Controlled demonstrations where only one specific object pair should matter.

### AnyCollisionLedController
File: Assets/Scripts/AnyCollisionLedController.cs

Responsibilities:
- Monitors one assigned collider against any valid collider in selected layers.
- Triggers red output only on collision entry (rising edge).
- Sends pulse duration to firmware for haptic-style output.

Key configurable fields:
- Monitored Collider
- Valid Layers
- QueryTriggerInteraction
- Red Led Pulse Duration Ms

Use case:
- Event-style contact feedback where one player object interacts with many world objects.

### BasicFirstPersonController
File: Assets/Scripts/BasicFirstPersonController.cs

Responsibilities:
- Keyboard walking and grounded jump for first-person movement.
- Movement aligned to camera facing direction on horizontal plane.

Required components:
- Rigidbody
- CapsuleCollider

Use case:
- Minimal first-person locomotion for classroom experimentation.

## 6. Firmware behavior reference
File: Firmware/UnitySerialController02/UnitySerialController02.ino

Input handling:
- Reads serial command lines into a fixed buffer.
- Handles CR/LF safely.
- Parses commands with sscanf.

Output behavior:
- Green channel follows Unity flashlight state.
- Red channel runs pulse timing logic for event feedback.

Pulse model:
- Command REDPULSE,durationMs starts a timed pulse window.
- Red output remains HIGH during active pulse window, then returns LOW.
- DEFAULT_RED_PULSE_MS is currently 250 and used as fallback for REDLED,1.

Why pulse mode:
- Better for haptic hardware behavior than sustained level control.
- Easier to tune feedback feel through duration.

## 7. Scene-level design assumptions
- Cylinder acts as player flashlight rig.
- Main Camera is childed to Cylinder for first-person perspective.
- Flashlight visual behavior and physical output are intentionally coupled for teaching clarity.

## 8. Controller selection guide
Choose FlashlightController when:
- You need sensor-driven rotation plus button-driven light toggle.

Choose CollisionLedController when:
- You need exactly two assigned colliders to drive collision output.

Choose AnyCollisionLedController when:
- You need one monitored object to react to collisions with any valid world object.
- You want event pulses suitable for haptic feedback.

## 9. Haptic feedback extension notes
Current red output logic is already structured for actuators beyond LEDs.

Recommended path for solenoid/tactile hardware:
1. Keep Unity event pulse semantics.
2. Tune pulse duration in Unity first.
3. Enforce electrical safety in hardware driver stage.
4. Add cooldown or rate limiting in firmware if rapid re-triggering causes heat/current issues.

Electrical caution:
- Do not drive high-current actuators directly from microcontroller pins.
- Use proper transistor/MOSFET driver stage, flyback protection, and suitable power supply.

## 10. Reliability and teaching trade-offs
This project intentionally balances robustness and readability.

Current trade-offs:
- Startup resend pattern is simple and easy to teach, but less rigorous than a full handshake/ack protocol.
- Bounds/overlap checks are straightforward for lessons, but not as semantically precise as dedicated physics event callbacks in all edge cases.
- String CSV protocol is easy to inspect, but binary framing would scale better for large systems.

## 11. Common troubleshooting
Serial opens in Unity but no changing values:
- Confirm Arduino Serial Monitor is closed.
- Confirm correct port name in DataIO.
- Confirm board is sending expected CSV format.

Green LED does not match Unity light on startup:
- Verify FlashlightController has valid DataIO and Target Light assignments.
- Ensure delayed startup resend timing is not removed.

Red pulse not firing:
- Verify AnyCollisionLedController has Monitored Collider assigned.
- Verify target objects are in Valid Layers.
- Verify colliders overlap physically.
- Enable outgoing message logs in DataIO and confirm REDPULSE messages appear.

No movement/jump:
- Confirm Rigidbody and CapsuleCollider on cylinder.
- Confirm Input axes are present in project input settings.
- Confirm ground layer mask includes floor surfaces.

## 12. Suggested lesson sequence
1. One-way serial input: pot + button into Unity.
2. Sensor-to-transform mapping in Unity.
3. Button-to-light toggle in Unity.
4. Unity-to-Arduino state mirroring (green LED).
5. Two-target collision output (red channel).
6. Any-target collision entry pulses (haptic-ready).
7. First-person navigation integration.
8. Actuator substitution exercise (LED to haptic hardware).

## 13. File map
- Assets/Scripts/ArduinoDataInputOutput.cs
- Assets/Scripts/FlashlightController.cs
- Assets/Scripts/CollisionLedController.cs
- Assets/Scripts/AnyCollisionLedController.cs
- Assets/Scripts/BasicFirstPersonController.cs
- Firmware/UnitySerialController02/UnitySerialController02.ino

## 14. Wiki usage note
This page is designed as a durable knowledge snapshot.

Recommended workflow:
- Keep README focused on quickstart and latest practical steps.
- Keep this wiki page focused on architecture, rationale, progression, and extension patterns.
