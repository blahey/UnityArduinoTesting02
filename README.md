# UnityArduinoTesting02 provides a simple example of Arduino-Unity communication, facilitating the use of sensors and actuators for game engine interactive experiences.

## This version demonstrates:
- Serial data communication from Arduino to Unity
- Data includes:
  - One analog sensor value (potentiometer)
  - One digital sensor state (button)
- `FlashlightController` maps potentiometer input to an absolute Z rotation on a selected target object
- `BasicFirstPersonController` enables keyboard walking and jumping from the first-person flashlight perspective
- Button input toggles a selected Light on and off
- Unity sends flashlight state back to Arduino so the green LED mirrors the Unity Light state
- Unity sends collision state back to Arduino so the red LED indicates Unity collision events
- Demonstrates bidirectional communication: sensor input into Unity, then Unity logic/data driving microcontroller outputs
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
- Unity sends newline-terminated command messages:
  - `FLASHLIGHT,0` or `FLASHLIGHT,1`
  - `REDLED,0` or `REDLED,1`
  - Example: `FLASHLIGHT,1`

## Interaction options for red LED control:
- Option 1: `CollisionLedController` (two specific objects)
  - Use when: You want collision detection only between one assigned object pair.
  - How it works: You assign `First Target` and `Second Target`. Red LED turns on only when those two overlap/collide.
  - Setup: Add `CollisionLedController` to a manager object, then assign `Data IO`, `First Target`, and `Second Target`.
- Option 2: `AnyCollisionLedController` (one object vs any valid object)
  - Use when: You want one assigned object to trigger red LED when it contacts any allowed scene object.
  - How it works: You assign `Monitored Collider` and choose `Valid Layers`. Red LED turns on when monitored object overlaps any collider in those layers.
  - Setup: Add `AnyCollisionLedController` to a manager object, then assign `Data IO`, `Monitored Collider`, and `Valid Layers`.
- Tip: Keep only one red LED controller active at a time to avoid conflicting commands.

## First-person movement option:
- Script: `BasicFirstPersonController`
- Use when: You want the cylinder flashlight rig to act as a simple player avatar.
- How it works:
  - Attach the script to the cylinder object.
  - Assign the child Main Camera transform as `View Transform`.
  - Uses keyboard movement (`Horizontal` and `Vertical`) and jump (`Jump`, default Space).
  - Movement direction follows camera facing on the horizontal plane.
- Required components on the cylinder:
  - `Rigidbody`
  - `CapsuleCollider`
- Recommended Rigidbody setup for this lesson:
  - `Use Gravity`: enabled
  - Rotation constraints: freeze X, Y, Z rotation (the script also enforces stable rotation behavior)

## About the LED example:
- The green LED is intentionally the simplest possible actuator output example.
- In the same pattern, Unity interactions, game logic, or data streams can control any actuator your microcontroller can drive, such as motors, relays, servos, buzzers, pumps, valves, LEDs, or addressable light strips.
- For this lesson, the LED keeps the hardware setup and debugging process simple while showing the full loop from Unity decision to physical output.

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
- Select the cylinder player object in the Hierarchy
  - Ensure `BasicFirstPersonController` is attached
  - Assign the child Main Camera to `View Transform`
  - Confirm the cylinder has `Rigidbody` and `CapsuleCollider`
- Hit Play in Unity
  - Look at the Console
    - `Serial port opened successfully.` should appear when Play starts
    - If `Target Light` is missing, `FlashlightController` reports an assignment error
  - Observe `Arduino Data Variables` in the DataIO Inspector
    - Values should update as you move the potentiometer and press the button
    - Unity must be the active window for values to visibly update in the Inspector
  - Interact with controls
    - `WASD` (or arrow keys) to walk
    - `Space` to jump
    - Potentiometer changes the target object's Z rotation (absolute angle mapping)
    - Button press toggles the target light on/off
    - Arduino green LED should mirror the Unity `Target Light` state
      - The startup state is sent immediately and then resent once shortly after Play starts to improve reliability during serial startup
    - Red LED controller options:
      - If using `CollisionLedController`, red LED turns on only for collisions between `First Target` and `Second Target`
      - If using `AnyCollisionLedController`, red LED turns on when `Monitored Collider` overlaps any object in `Valid Layers`
- Hit Stop in Unity
  -  `Serial port closed` should appear when game is stopped.

## Lesson checkpoint
- Checkpoint 1: Serial input is live
  - In Play mode, confirm `Arduino Data Variables` update when you move the potentiometer and press/release the button.
- Checkpoint 2: First-person movement works
  - Use keyboard controls to walk from the first-person camera perspective.
  - Press `Space` and verify jumping only occurs when grounded.
- Checkpoint 3: Potentiometer controls rotation
  - Move the potentiometer slowly and verify the assigned `Target Object` rotates about Z through the expected angle range.
- Checkpoint 4: Button toggles light state
  - Press and release the button once to toggle `Target Light` on.
  - Press and release again to toggle `Target Light` off.
- Checkpoint 5: Unity output controls microcontroller actuator
  - At Play start, verify Arduino green LED matches the initial `Target Light` state.
  - After each button toggle in Unity, verify the green LED updates to the same on/off state.
- Checkpoint 6: Collision logic controls red LED
  - `CollisionLedController` path: move either assigned target into collision with the other and verify red LED turns on; separate them and verify red LED turns off.
  - `AnyCollisionLedController` path: move the monitored collider into any valid target layer object and verify red LED turns on; separate them and verify red LED turns off.




