# UnityArduinoTesting02 Wiki Home

This wiki captures the cumulative design knowledge behind the Unity + Arduino teaching project.

Use this page as the quick index, then open topic pages for implementation details.

## Quick overview
- Unity reads sensor data from Arduino (potentiometer + button).
- Unity applies game logic (flashlight rotation, light toggle, movement, collision logic).
- Unity sends actuator commands back to Arduino (green light state + red pulse events).
- Arduino firmware maps Unity commands to physical output pins.

## Recommended reading order
1. [Project-Knowledge-Base](Project-Knowledge-Base.md)
2. [Unity-Scripts](Unity-Scripts.md)
3. [Firmware-and-Serial-Protocol](Firmware-and-Serial-Protocol.md)
4. [Interaction-Modes-and-Controllers](Interaction-Modes-and-Controllers.md)
5. [Lesson-Sequence-and-Classroom-Notes](Lesson-Sequence-and-Classroom-Notes.md)

## Current capabilities snapshot
- Sensor input: analog potentiometer + digital button.
- Flashlight controller: absolute Z rotation + button light toggle.
- First-person movement: keyboard walking + jump.
- Collision outputs:
  - Two-target collision option.
  - Any-valid-target collision-entry pulse option.
- Haptic-ready output pattern: adjustable red pulse duration.

## Hardware pin map
- A0: potentiometer input
- D2: button input (active low)
- D7: green output (flashlight state)
- D8: red output (collision pulse)

## Maintainer note
- Keep README focused on quickstart and latest setup changes.
- Keep wiki pages focused on architecture, rationale, extension patterns, and teaching progression.
