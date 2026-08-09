# Firmware and Serial Protocol

This page documents the Arduino sketch behavior and the message contract with Unity.

Primary sketch:
- [Firmware/UnitySerialController02/UnitySerialController02.ino](../../Firmware/UnitySerialController02/UnitySerialController02.ino)

## Serial configuration
- Baud rate: 115200
- Line framing: newline-terminated messages
- Unity should keep Arduino IDE Serial Monitor closed while running

## Arduino -> Unity messages
Format:
- potValue,buttonState

Examples:
- 0,0
- 512,1
- 1023,0

Fields:
- potValue: expected analog range 0-1023
- buttonState: 0 or 1 (button pin is active low in firmware)

## Unity -> Arduino messages
Supported commands:
- FLASHLIGHT,0
- FLASHLIGHT,1
- REDPULSE,durationMs
- REDLED,1 (legacy compatibility trigger)

Command behavior:
- FLASHLIGHT controls green output channel state.
- REDPULSE starts timed red pulse output.
- REDLED,1 triggers default red pulse duration.

## Output timing model
Green output:
- State-based. Mirrors Unity light state.

Red output:
- Event-based pulse model.
- Pulse active window is tracked by end timestamp.
- LED is HIGH only while pulse window is active.

Key constants:
- SEND_INTERVAL_MS: sensor send interval to Unity.
- DEFAULT_RED_PULSE_MS: fallback pulse length (currently 250 ms).

## Why pulse-based red output
- Better fit for haptic actuators than sustained level control.
- Gives clear event semantics: one collision entry -> one pulse.
- Easy to tune by changing duration without rewriting logic.

## Extension ideas
- Add per-channel cooldown logic to prevent actuator overheating.
- Add message acknowledgments for robust startup synchronization.
- Add CRC/checksum framing if scaling to noisier links.
