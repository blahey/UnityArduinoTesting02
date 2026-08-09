# Unity Scripts

This page describes each Unity runtime script, its role, and how scripts connect.

## Script map
- [Assets/Scripts/ArduinoDataInputOutput.cs](../../Assets/Scripts/ArduinoDataInputOutput.cs)
- [Assets/Scripts/FlashlightController.cs](../../Assets/Scripts/FlashlightController.cs)
- [Assets/Scripts/BasicFirstPersonController.cs](../../Assets/Scripts/BasicFirstPersonController.cs)
- [Assets/Scripts/CollisionLedController.cs](../../Assets/Scripts/CollisionLedController.cs)
- [Assets/Scripts/AnyCollisionLedController.cs](../../Assets/Scripts/AnyCollisionLedController.cs)

## ArduinoDataInputOutput
Purpose:
- Owns serial port lifecycle.
- Reads incoming sensor lines.
- Exposes parsed values for other scripts.
- Sends outbound command lines to firmware.

Inbound parsed fields:
- potValue
- buttonPressed

Outbound commands:
- SendFlashlightState(bool) -> FLASHLIGHT,0|1
- SendRedLedState(bool) -> REDLED,0|1 (legacy style)
- SendRedLedPulse(int) -> REDPULSE,durationMs

Notes:
- Uses read/write timeouts.
- Uses Trim + TryParse pattern for stability.
- Optional logging via logOutgoingMessages.

## FlashlightController
Purpose:
- Maps potentiometer value to absolute Z rotation on assigned target object.
- Toggles assigned Unity Light on button rising edge.
- Syncs Unity light state to Arduino green output.

Setup requirements:
- Data IO assigned.
- Target Object assigned (or defaults to attached object).
- Target Light assigned explicitly.

Behavior details:
- Uses edge detection for button press to avoid repeated toggles while held.
- Uses startup immediate-send + delayed resend for serial startup reliability.

## BasicFirstPersonController
Purpose:
- Provides simple first-person walking and jump logic for the cylinder/player rig.

Required components:
- Rigidbody
- CapsuleCollider

Movement model:
- Horizontal/Vertical input for movement.
- Jump input for grounded jump.
- Direction is projected from camera forward/right onto horizontal plane.

## CollisionLedController
Purpose:
- Monitors collision state between two specifically assigned colliders.
- Mirrors state to red output command path.

Best use case:
- Early lessons where students should reason about one known interaction pair.

## AnyCollisionLedController
Purpose:
- Monitors one assigned collider against any valid collider in selected layers.
- Fires only on collision entry (rising edge), not continuously.
- Sends pulse command with configurable duration.

Best use case:
- Event-driven physical feedback patterns.
- Haptic-ready interactions.

## Common Unity wiring pattern
1. Add one DataIO object with ArduinoDataInputOutput.
2. Add one gameplay controller object for flashlight behavior.
3. Add one movement controller object (or attach to player cylinder).
4. Add one red-output collision controller (choose one type at a time).
