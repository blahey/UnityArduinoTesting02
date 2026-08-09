# Interaction Modes and Controllers

This page compares interaction controller options and when to use each.

## Flashlight interaction mode
Controller:
- FlashlightController

Inputs:
- Potentiometer value
- Button state

Outputs:
- Target object Z rotation
- Target Unity Light toggle
- Green hardware output mirror

Use when:
- Teaching direct mapping from sensor input to visual + physical output.

## Collision mode A: specific pair
Controller:
- CollisionLedController

Model:
- Two explicitly assigned colliders.
- State-style on/off reflection for the selected pair.

Use when:
- Students are learning controlled condition checks with minimal ambiguity.

## Collision mode B: one-vs-any valid target
Controller:
- AnyCollisionLedController

Model:
- One monitored collider.
- Layer-filtered detection against any valid collider.
- Entry-only event trigger.
- Pulse command output with adjustable duration.

Use when:
- You want scalable world interactions.
- You want event pulses suitable for haptic/tactile feedback.

## First-person movement mode
Controller:
- BasicFirstPersonController

Model:
- Keyboard movement and grounded jump.
- Camera-relative direction on horizontal plane.

Use when:
- Students need embodied first-person navigation in the test environment.

## Controller coexistence guidance
- Keep exactly one red-output controller active at a time.
- Flashlight and movement controllers can run together.
- Recheck target assignments after scene changes.

## Suggested incremental progression
1. Flashlight only
2. Flashlight + green output mirror
3. Add collision mode A
4. Replace with collision mode B
5. Tune pulse for haptic hardware
