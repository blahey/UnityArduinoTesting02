#include <elapsedMillis.h>

const int POT_PIN = A0;
const int BUTTON_PIN = 2;
const int GREEN_LED_PIN = 7;
const int RED_LED_PIN = 8;
const unsigned long SEND_INTERVAL_MS = 50;

elapsedMillis sendTimer;
bool flashlightState = false;
bool redLedState = false;

const int SERIAL_BUFFER_SIZE = 32;
char serialBuffer[SERIAL_BUFFER_SIZE];
int serialBufferIndex = 0;

void handleIncomingCommand(const char* command) {
  int stateValue = 0;

  if (sscanf(command, "FLASHLIGHT,%d", &stateValue) == 1) {
    flashlightState = stateValue != 0;
  } else if (sscanf(command, "REDLED,%d", &stateValue) == 1) {
    redLedState = stateValue != 0;
  }
}

void processIncomingSerial() {
  while (Serial.available() > 0) {
    char incomingChar = Serial.read();

    if (incomingChar == '\r') {
      continue;
    }

    if (incomingChar == '\n') {
      serialBuffer[serialBufferIndex] = '\0';

      if (serialBufferIndex > 0) {
        handleIncomingCommand(serialBuffer);
      }

      serialBufferIndex = 0;
    } else if (serialBufferIndex < SERIAL_BUFFER_SIZE - 1) {
      serialBuffer[serialBufferIndex++] = incomingChar;
    } else {
      // If a line is too long, reset and wait for the next command.
      serialBufferIndex = 0;
    }
  }
}

void setup() {
  pinMode(POT_PIN, INPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  pinMode(GREEN_LED_PIN, OUTPUT);
  pinMode(RED_LED_PIN, OUTPUT);

  Serial.begin(115200);
}

void readSensors() {
  int potValue = analogRead(POT_PIN);
  bool buttonState = digitalRead(BUTTON_PIN) == LOW; // Active low button

  // Send one newline-terminated CSV message: potValue,button(0|1)
  Serial.print(potValue);
  Serial.print(',');
  Serial.println(buttonState ? 1 : 0);

  // Green LED mirrors Unity flashlight state.
  digitalWrite(GREEN_LED_PIN, flashlightState ? HIGH : LOW);

  // Red LED reflects Unity collision state.
  digitalWrite(RED_LED_PIN, redLedState ? HIGH : LOW);
} 

void loop() {
  processIncomingSerial();

  if (sendTimer >= SEND_INTERVAL_MS) {
    sendTimer = 0; // Reset the timer
    readSensors(); // Read and send sensor data
  }
}
