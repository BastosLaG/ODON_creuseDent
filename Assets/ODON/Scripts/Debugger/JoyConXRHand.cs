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
    // private UnscentedKalmanFilter.UKF ukfJoycon;

    [SerializeField] Transform targetTransform;

    [Range(0, 360)]
    [SerializeField] private double rotationValue = 90.0;

    private Queue<double> gyroBuffer = new();
    private Queue<Vector3> gyroThresholdBuffer = new();
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
        joyConLeft = null;
        joyConRight = null;

        // ukfJoycon = new UnscentedKalmanFilter.UKF();

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
        if (device is not SwitchJoyConLHID && device is not SwitchJoyConRHID)
        {
            return;
        }
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        {
            Debug.Log("Event is not a StateEvent or DeltaStateEvent, returning.");
            return;
        }

        Vector3 gyro = GetGyroscope(eventPtr, device);
        Vector3 accel = GetAcceleration(eventPtr, device);

        targetTransform.rotation = Quaternion.Euler(gyro);
    }


    private Vector3 GetGyroscope(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            if (leftJoyCon.dpad.ReadValueFromEvent(eventPtr)[1] == -1)
            {
                Debug.Log("Button South pressed on left Joy-Con. Starting calibration.");
                isRecording = true;
            }

            return leftJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            if (rightJoyCon.buttonSouth.ReadValueFromEvent(eventPtr) == 1)
            {
                Debug.Log("Button South pressed on right Joy-Con. Starting calibration.");
                isRecording = true;
            }

            return rightJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }

        return Vector3.zero;
    }

    private Vector3 GetAcceleration(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            return leftJoyCon.acceleration.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            return rightJoyCon.acceleration.ReadValueFromEvent(eventPtr);
        }

        return Vector3.zero;
    }

}