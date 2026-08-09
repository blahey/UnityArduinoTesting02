/* This script is used for handling Arduino data input and output in Unity.
 * It establishes a serial connection with the Arduino and facilitates data exchange.
 * It reads data from the Arduino and updates the corresponding Unity variables.

*/

using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoDataInputOutput : MonoBehaviour
{
    [Header("Arduino Data Input/Output Settings")]
    public string portName = "/dev/cu.usbmodem21101"; // Change this to your Arduino's port name
    public int baudRate = 115200; // Change this to your Arduino's baud rate

    private SerialPort stream;

    [Header("Arduino Data Variables")]
    public int potValue;
    public bool buttonPressed;

    [Header("Arduino Data Output")]
    public bool logOutgoingMessages;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the serial port
        stream = new SerialPort(portName, baudRate);
        stream.ReadTimeout = 50; // Set a read timeout (in milliseconds)
        stream.WriteTimeout = 50; // Set a write timeout (in milliseconds)

        try
        {
            stream.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    // Sends flashlight state to Arduino as newline-terminated CSV: FLASHLIGHT,0|1
    public void SendFlashlightState(bool isOn)
    {
        if (stream == null || !stream.IsOpen)
        {
            return;
        }

        string message = "FLASHLIGHT," + (isOn ? "1" : "0");

        try
        {
            stream.WriteLine(message);

            if (logOutgoingMessages)
            {
                Debug.Log("Unity -> Arduino: " + message);
            }
        }
        catch (TimeoutException)
        {
            Debug.LogWarning("Timed out writing to serial port.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing to serial port: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stream != null && stream.IsOpen)
        {
            try
            {
                // Read data from the Arduino
                string data = stream.ReadLine();
                if (string.IsNullOrWhiteSpace(data))
                {
                    return;
                }

                string[] values = data.Trim().Split(',');

                if (values.Length == 2 &&
                    int.TryParse(values[0].Trim(), out int parsedPot) &&
                    int.TryParse(values[1].Trim(), out int parsedButton))
                {
                    potValue = parsedPot;
                    buttonPressed = parsedButton != 0;
                }
            }
            catch (TimeoutException)
            {
                // Handle timeout exception if no data is received within the specified time
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading from serial port: " + e.Message);
            }
        }
    }
    void OnApplicationQuit()
    {
        // Close the serial port when the application quits
        if (stream != null && stream.IsOpen)
        {
            stream.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
