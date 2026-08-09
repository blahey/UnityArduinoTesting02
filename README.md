# UnityArduinoTesting02 provides a simple example of Arduino-Unity communication, facilitating the use of sensors and actuators for game engine interactive experiences.

## This version demonstrates:
- Serial data communication from Arduino to Unity
- Data includes:
  - One analog sensor value (potentiometer)
  - One digital sensor state (button)
- `FlashlightController` maps potentiometer input to an absolute Z rotation on a selected target object
- Button input toggles a selected Light on and off
- Unity scene includes several objects to produce a flashlight experience

## Hardware requirements:
- Arduino compatible microcontroller
- This example was tested on an Elegoo Arduino Mega
- Analog sensor on pin A0 (tested with potentiometer)
- Digital sensor on pin 2 (tested with momentary SPST switch on breadboard)

## Library requirements:
- `elapsedMillis` by Paul Stoffregen (tested with version 1.0.6)

## Unity requirements:
- Unity 6000.0.64f1 or later
- Edit > Project Settings > Player > Configuration
  - Set API Compatibility Level to `.NET Framework`

## Serial data format:
- Firmware sends newline-terminated CSV:
  - `potValue,buttonState`
  - Example: `512,1`

## Testing process:
### Arduino
- Assemble Arduino circuit
- Upload sketch to Arduino
- Test output with Arduino Serial Monitor
- Close Serial Monitor (this is essential for Unity to open the same serial connection)

### Unity
- Open Unity project
- Set API Compatibility (see Unity requirements)
- Select the DataIO object in the Hierarchy
  - Set your `Port Name` in the Inspector (find it in the Arduino IDE)
- Select the FlashlightControl object in the Hierarchy (or your controller object)
  - Ensure the `FlashlightController` script is attached
  - Assign `Data IO`
  - Assign `Target Object` (the object that should rotate)
  - Assign `Target Light` (the Light component to toggle)
  - Note: `Target Light` must be assigned manually. The script intentionally reports an error if it is missing.
- Hit Play in Unity
  - Look at the Console
    - `Serial port opened successfully.` should appear when Play starts
    - If `Target Light` is missing, `FlashlightController` reports an assignment error
  - Observe `Arduino Data Variables` in the DataIO Inspector
    - Values should update as you move the potentiometer and press the button
    - Unity must be the active window for values to visibly update in the Inspector
  - Interact with controls
    - Potentiometer changes the target object's Z rotation (absolute angle mapping)
    - Button press toggles the target light on/off
- Hit Stop in Unity
  -  `Serial port closed` should appear when game is stopped.

## Lesson checkpoint
- Checkpoint 1: Serial input is live
  - In Play mode, confirm `Arduino Data Variables` update when you move the potentiometer and press/release the button.
- Checkpoint 2: Potentiometer controls rotation
  - Move the potentiometer slowly and verify the assigned `Target Object` rotates about Z through the expected angle range.
- Checkpoint 3: Button toggles light state
  - Press and release the button once to toggle `Target Light` on.
  - Press and release again to toggle `Target Light` off.




