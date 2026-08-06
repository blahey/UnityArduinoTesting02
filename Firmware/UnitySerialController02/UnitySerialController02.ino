#include <elapsedMillis.h>

const int POT_PIN = A0;
const int BUTTON_PIN = 2;
const int GREEN_LED_PIN = 7;
const int RED_LED_PIN = 8;
const unsigned long SEND_INTERVAL_MS = 50;

elapsedMillis sendTimer;

void setup() {
  pinMode(POT_PIN, INPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  pinMode(GREEN_LED_PIN, OUTPUT);
  pinMode(RED_LED_PIN, OUTPUT);

  Serial.begin(115200);
  Serial.println("Unity Serial Controller Initialized");
}

void readSensors() {
  int potValue = analogRead(POT_PIN);
  bool buttonState = digitalRead(BUTTON_PIN) == LOW; // Active low button

  // Send data over serial
  Serial.print("Potentiometer: ");
  Serial.print(potValue);
  Serial.print(", Button: ");
  Serial.println(buttonState ? "Pressed" : "Released");

  // Update LEDs based on button state
  digitalWrite(GREEN_LED_PIN, buttonState ? HIGH : LOW);
  digitalWrite(RED_LED_PIN, buttonState ? LOW : HIGH);
} 

void loop() {
         if (sendTimer >= SEND_INTERVAL_MS) {
             sendTimer = 0; // Reset the timer
             readSensors(); // Read and send sensor data    
  }
}
