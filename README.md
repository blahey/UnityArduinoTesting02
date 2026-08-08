# UnityArduinoTesting02 provides a simple example of Arduino-Unity communication, facilitating the use of sensors and acuators for an extended range of game engine interactive experiences. 

## This version demonstrates:
- Serial data communication from the Arduio to Unity
- Data includes:
  -  One analog sensor value
  - One digital sensor state
- Implements the use of potentiometer data to rotate a targeted object in Unity
- Unity scene includes several objects to produce a flashlight experience



## Hardware requirements:
- Arduino compatible microcontroller
- This example was testing on an Ellegoo Arduino Mega
- Analog sensor on pin A0 (tested with potentiometer)    
- Digital sensor on pin 2 (tested with momentary SPST switch on breadboard)

## Libray Requirements
- elapsedMillis by Paul Stoffregen (tested with version 1.0.6 )    

## Unity Requirements:
- Unity 6000.0.64f1 or later
- Edit > Project Settings > Player > Configuration
  - Set Api Compatibiity Level to ".NET Framework"

## Testing process:
### Arduino
- Assemble Arduino circuit
- Upload sketdh to Arduino
- Test output with Arduino Serial Monitor
- Close Serial Monitor (this is essential for Unity to see the connection)  
### Unity
- Open Unity Project
- Set Api Compatibility (see Unity Requirements)      
- Select the DataIO object in the Hierarchy
  - Set your Port Name in the Inspector window (view your port in the Arduino IDE)
  - Hit Play (in Unity)
  - Look at Console in Unity
    - "Serial port opened successfully." should appear when you hit play.
    - Observe "Arduino Data Varaibles" in the DataIO inspector view.
      - These values should update when you interact with your sensors.
      - Note: Unity must be the active (selected) window for the display to update.
  - Select FlashlightControl object in Heirarchy
    - Assign an object to control by dragging it to the Target Object field. (Experiment with different objects!)
    - Read the code and comments in the FlashlightController script to understand how it works. 
    - Try changing variables in FlashlightControl. 
- Hit Stop (in Unity)




