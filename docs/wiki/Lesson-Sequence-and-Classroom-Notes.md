# Lesson Sequence and Classroom Notes

This page provides a teaching-oriented progression path and discussion prompts.

## Suggested sequence
1. Sensor intake
- Verify Unity receives potentiometer and button values.
- Discussion: polling, parsing, and error tolerance.

2. Visual mapping
- Map pot range to absolute object rotation.
- Discussion: normalization and range mapping.

3. State toggle logic
- Toggle Unity light on button rising edge.
- Discussion: state machines and edge-trigger logic.

4. Unity -> hardware mirror
- Mirror Unity light state to green output.
- Discussion: one source of truth and startup synchronization.

5. Collision output (specific pair)
- Use CollisionLedController with two assigned targets.
- Discussion: deterministic conditions and debugging.

6. Collision output (any valid target)
- Use AnyCollisionLedController and layer filtering.
- Discussion: scalability and filtering strategy.

7. Pulse-based haptic pattern
- Trigger REDPULSE on collision entry.
- Discussion: event-driven output vs sustained state.

8. First-person integration
- Move/jump with flashlight player rig.
- Discussion: tying movement to interactive physical feedback.

## Lab prompts
- What changes if collisions are continuous vs entry-only?
- How would you prevent repeated pulses while sliding across surfaces?
- What is the safest way to drive a solenoid from a microcontroller pin?
- Which parameters should be student-exposed in Inspector vs firmware constants?

## Assessment checkpoints
- Students can explain message formats in both directions.
- Students can wire target references correctly in Inspector.
- Students can modify pulse duration and predict output behavior.
- Students can justify when to choose specific-pair vs one-vs-any collision mode.

## Practical safety note
For tactile actuators:
- Use proper switching hardware (transistor/MOSFET driver).
- Add flyback diode for inductive loads.
- Validate current draw and duty cycle before extended operation.
