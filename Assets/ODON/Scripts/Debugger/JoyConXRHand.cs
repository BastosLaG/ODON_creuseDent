using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;
using System.Collections.Generic;
using System;

// * https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputDevice.html

public class JoyConXRHand : MonoBehaviour
{
    public bool isLeftHand = true;
    private InputDevice joyConLeft;
    private InputDevice joyConRight;

    /// <summary>
    /// Unscented Kalman Filter Instance
    /// </summary>
    private UnscentedKalmanFilter.UKF ukfJoyconLeft;
    private UnscentedKalmanFilter.UKF ukfJoyconRight;

    [SerializeField] Transform targetTransform;

    [Range(0, 360)]
    [SerializeField] private double rotationValue = 90.0;

    private Queue<double> gyroBuffer = new ();
    private Queue<Vector3> gyroThresholdBuffer = new ();
    private Vector3 gyroMaxSizeThreshold;
    private Vector3 gyroMinSizeThreshold;
    private bool isRecording = false;
    private const int bufferThresholdSize = 10;
    private Queue<double> accelBuffer = new();


    private const int bufferSize = 10;


    private void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = transform;
        }

        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        joyConLeft = null;
        joyConRight = null;

        ukfJoyconLeft = new UnscentedKalmanFilter.UKF(); 
        ukfJoyconRight = new UnscentedKalmanFilter.UKF(); 

        foreach (var device in InputSystem.devices)
        {
            if (isLeftHand && device is SwitchJoyConLHID left)
            {
                left.SetLEDs(LEDStatusEnum.On);
                bool success = left.SetIMUEnabled(true);
                if (!success)
                {
                    Debug.LogError("Failed to enable IMU on Switch controller.");
                }
                joyConLeft = left;
                Debug.Log("Joy-Con left detect !");
            }
            else if (!isLeftHand && device is SwitchJoyConRHID right)
            {
                right.SetLEDs(LEDStatusEnum.On);
                right.SetIMUEnabled(true);
                bool success = right.SetIMUEnabled(true);
                if (!success)
                {
                    Debug.LogError("Failed to enable IMU on Switch controller.");
                }
                joyConRight = right;
                Debug.Log("Joy-Con right detect !");
            }
        }
    }

    void OnEnable()
    {
        // InputSystem.onEvent += OnInputEventReadGamePad;
        InputSystem.onEvent += OnInputEventReadJoycon;
    }

    void OnDisable()
    {
        // InputSystem.onEvent -= OnInputEventReadGamePad;
        InputSystem.onEvent -= OnInputEventReadJoycon;

        joyConLeft = null;
        joyConRight = null;

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device: {device.displayName} - Type: {device.GetType()}");
            if (device is SwitchJoyConLHID left)
            {
                left.SetLEDs(LEDStatusEnum.Flashing);
                Debug.Log("Joy-Con left detect !");
            }
            else if (device is SwitchJoyConRHID right)
            {
                right.SetLEDs(LEDStatusEnum.Flashing);
                Debug.Log("Joy-Con right detect !");
            }
        }
    }

    private void OnInputEventReadJoycon(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is not SwitchJoyConLHID or SwitchJoyConRHID)
        {
            return;
        }
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        {
            Debug.Log("Event is not a StateEvent or DeltaStateEvent, returning.");
            return;
        }

        Vector3 gyro = Vector3.zero;
        Vector3 accel = Vector3.zero;
        if (device is SwitchJoyConLHID left)
        {
            Vector3 tempGyro = left.angularVelocity.ReadValueFromEvent(eventPtr);
            if (gyroThresholdBuffer.Count >= bufferSize)
            {
                gyroMaxSizeThreshold = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                gyroMinSizeThreshold = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                foreach (var item in gyroThresholdBuffer)
                {
                    gyroMaxSizeThreshold = Vector3.Max(gyroMaxSizeThreshold, item);
                    gyroMinSizeThreshold = Vector3.Min(gyroMinSizeThreshold, item);
                }
            }
            if (isRecording)
            {
                gyroThresholdBuffer.Enqueue(tempGyro);
                return;
            }
            if (left.buttonSouth.ReadValueFromEvent(eventPtr) == 1) // Check if the South button is pressed
            {
                Debug.Log("Button South pressed on left Joy-Con. Starting calibration.");
                isRecording = true;
                gyroThresholdBuffer.Clear();
                gyroMaxSizeThreshold = Vector3.zero;
                gyroMinSizeThreshold = Vector3.zero;
            }

            // Read gyro and accel data from the left Joy-Con
            if (tempGyro.x < gyroMinSizeThreshold.x || tempGyro.x > gyroMaxSizeThreshold.x)
                gyro.x = tempGyro.x;
            if (tempGyro.y < gyroMinSizeThreshold.y || tempGyro.y > gyroMaxSizeThreshold.y)
                gyro.y = tempGyro.y;   
            if (tempGyro.z < gyroMinSizeThreshold.z || tempGyro.z > gyroMaxSizeThreshold.z)
                gyro.z = tempGyro.z;
            
            accel = left.acceleration.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID right)
        {
            Vector3 tempGyro = right.angularVelocity.ReadValueFromEvent(eventPtr);
            if (gyroThresholdBuffer.Count >= bufferSize)
            {
                gyroMaxSizeThreshold = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                gyroMinSizeThreshold = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                foreach (var item in gyroThresholdBuffer)
                {
                    gyroMaxSizeThreshold = Vector3.Max(gyroMaxSizeThreshold, item);
                    gyroMinSizeThreshold = Vector3.Min(gyroMinSizeThreshold, item);
                }
            }
            if (isRecording)
            {
                gyroThresholdBuffer.Enqueue(tempGyro);
                return;
            }
            if (right.buttonSouth.ReadValueFromEvent(eventPtr) == 1) // Check if the South button is pressed
            {
                Debug.Log("Button South pressed on left Joy-Con. Starting calibration.");
                isRecording = true;
                gyroThresholdBuffer.Clear();
                gyroMaxSizeThreshold = Vector3.zero;
                gyroMinSizeThreshold = Vector3.zero;
            }

            // Read gyro and accel data from the left Joy-Con
            if (tempGyro.x < gyroMinSizeThreshold.x || tempGyro.x > gyroMaxSizeThreshold.x)
                gyro.x = tempGyro.x;
            if (tempGyro.y < gyroMinSizeThreshold.y || tempGyro.y > gyroMaxSizeThreshold.y)
                gyro.y = tempGyro.y;   
            if (tempGyro.z < gyroMinSizeThreshold.z || tempGyro.z > gyroMaxSizeThreshold.z)
                gyro.z = tempGyro.z;
            
            accel = right.acceleration.ReadValueFromEvent(eventPtr);
        }

        //Create a buffer to store the last 10 values of gyro and accel
        // and use them to calculate the average for smoothing
        // Add to gyro buffer
        gyroBuffer.Enqueue(gyro.magnitude);
        if (gyroBuffer.Count > bufferSize)
            gyroBuffer.Dequeue();

        // Add to accel buffer
        accelBuffer.Enqueue(accel.magnitude);
        if (accelBuffer.Count > bufferSize)
            accelBuffer.Dequeue();

        if (isLeftHand && gyro != null)
        {
            ukfJoyconLeft.Update(gyroBuffer.ToArray());
            targetTransform.rotation = Quaternion.Euler((float)(ukfJoyconLeft.GetState()[0] * rotationValue),
                                                        (float)(ukfJoyconLeft.GetState()[1] * rotationValue),
                                                        (float)(ukfJoyconLeft.GetState()[2] * rotationValue));
        }
        else if (!isLeftHand && gyro != null)
        {
            ukfJoyconRight.Update(gyroBuffer.ToArray());
            targetTransform.rotation = Quaternion.Euler((float)(ukfJoyconRight.GetState()[0] * rotationValue),
                                                        (float)(ukfJoyconRight.GetState()[1] * rotationValue),
                                                        (float)(ukfJoyconRight.GetState()[2] * rotationValue));
        }
    }
}