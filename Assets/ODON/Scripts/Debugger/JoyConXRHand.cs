using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;
using System.Collections.Generic;
using System;

// * https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputDevice.html
/// <summary>
/// This script connects a Nintendo Switch Joy-Con controller (left or right)
/// to a Unity GameObject and uses its IMU data (orientation, angular velocity, acceleration)
/// to rotate the object in real time.
/// </summary>
public class JoyConXRHand : MonoBehaviour
{
    [Header("Joy-Con Settings")]

    [Tooltip("Set to true if using the Left Joy-Con, false for Right Joy-Con.")]
    public bool isLeftHand = true;

    private InputDevice joyConLeft;
    private InputDevice joyConRight;

    [Header("Target Settings")]

    [Tooltip("The transform that will be rotated by Joy-Con input.")]
    [SerializeField] private Transform targetTransform;

    [Tooltip("Optional rotation scaling or clamping (not used in current logic).")]
    [Range(0, 360)]
    [SerializeField] private double rotationValue = 90.0;

    #region Primary Function
    private void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = transform;
        }

        joyConLeft = null;
        joyConRight = null;

        // ukfJoycon = new UnscentedKalmanFilter.UKF();

        // Find and initialize the correct Joy-Con
        foreach (var device in InputSystem.devices)
        {
            if (isLeftHand && device is SwitchJoyConLHID left)
            {
                left.SetLEDs(LEDStatusEnum.On);
                bool success = left.SetIMUEnabled(true);
                if (!success)
                {
                    Debug.LogWarning("Failed to enable IMU on Switch controller.");
                }
                left.CalibrateJoycon();
                joyConLeft = left;

                // Debug.Log("Joy-Con left detect !");
            }
            else if (!isLeftHand && device is SwitchJoyConRHID right)
            {
                right.SetLEDs(LEDStatusEnum.On);
                right.SetIMUEnabled(true);
                bool success = right.SetIMUEnabled(true);
                if (!success)
                {
                    Debug.LogWarning("Failed to enable IMU on Switch controller.");
                }
                right.CalibrateJoycon();
                joyConRight = right;
                // Debug.Log("Joy-Con right detect !");
            }
        }
    }

    void OnEnable()
    {
        InputSystem.onEvent += OnInputEventReadJoycon;
    }

    void OnDisable()
    {
        InputSystem.onEvent -= OnInputEventReadJoycon;

        joyConLeft = null;
        joyConRight = null;

        // Flash LEDs to indicate disconnection
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
    #endregion

    #region Logic Function

    /// <summary>
    /// Called when input events are received from any device.
    /// Filters for Joy-Con events and applies angular velocity to rotate the target transform.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
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

        

        GetOrientation(eventPtr, device, out Vector3 orientation);
        GetAcceleration(eventPtr, device, out Vector3 acceleration);
        GetAngularVelocity(eventPtr, device, out Vector3 angularVelocity);

        SetRotationAndPosition(angularVelocity, acceleration, orientation);
    }

    private void SetRotationAndPosition(Vector3 angularVelocity, Vector3 orientation, Vector3 acceleration)
    {
        // TODO : implement logic rotation here 
        targetTransform.rotation *= Quaternion.Euler(angularVelocity * Time.deltaTime);
    }
    #endregion

    #region Getter

    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    private Vector3 GetOrientation(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            return leftJoyCon.orientation.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            return rightJoyCon.orientation.ReadValueFromEvent(eventPtr);
        }

        return Vector3.zero;
    }
    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <param name="orientation"></param>
    private void GetOrientation(InputEventPtr eventPtr, InputDevice device, out Vector3 orientation)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            orientation = leftJoyCon.orientation.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            orientation = rightJoyCon.orientation.ReadValueFromEvent(eventPtr);
        }
        else
        {
            orientation = Vector3.zero;
        }
    }

    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    private Vector3 GetAngularVelocity(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            return leftJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            return rightJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <param name="angularVelocity"></param>
    private void GetAngularVelocity(InputEventPtr eventPtr, InputDevice device, out Vector3 angularVelocity)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            angularVelocity = leftJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            angularVelocity = rightJoyCon.angularVelocity.ReadValueFromEvent(eventPtr);
        }
        else
        {
            angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <param name="acceleration"></param>
    private void GetAcceleration(InputEventPtr eventPtr, InputDevice device, out Vector3 acceleration)
    {
        if (device is SwitchJoyConLHID leftJoyCon)
        {
            acceleration = leftJoyCon.acceleration.ReadValueFromEvent(eventPtr);
        }
        else if (device is SwitchJoyConRHID rightJoyCon)
        {
            acceleration = rightJoyCon.acceleration.ReadValueFromEvent(eventPtr);
        }
        else
        {
            acceleration = Vector3.zero;
        }
    }
    #endregion
}