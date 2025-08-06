using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

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

    [SerializeField] private SwitchControllerHID _joyCon;

    [Header("Target Settings")]

    [Tooltip("The transform that will be rotated by Joy-Con input.")]
    [SerializeField] private Transform _targetTransform;

    [Tooltip("Optional rotation scaling or clamping (not used in current logic).")]
    [Range(0, 360)]
    [SerializeField] private double _rotationValue = 90.0;
    [Tooltip("Time we need before calibration")]
    [Range(0.0f, 5.0f)]
    [SerializeField] private float _timer = 2.0f;

    #region Primary Function
    private void Start()
    {
        if (_targetTransform == null)
        {
            _targetTransform = transform;
        }

        _joyCon = null;

        // Find and initialize the correct Joy-Con
        foreach (var device in InputSystem.devices)
        {
            if (isLeftHand && device is SwitchJoyConLHID left)
            {
                SetupJoyCon(left);
                // Debug.Log("Joy-Con left detect !");
                return;
            }
            else if (!isLeftHand && device is SwitchJoyConRHID right)
            {
                SetupJoyCon(right);
                // Debug.Log("Joy-Con right detect !");
                return;
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

        _joyCon = null;

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
            Debug.LogError("Event is not a StateEvent or DeltaStateEvent, returning.");
            return;
        }

        if (_joyCon == null)
        {
            Debug.LogError("Joycon is null");
        }
        GetOrientation(eventPtr, out Vector3 orientation);
        GetAcceleration(eventPtr, out Vector3 acceleration);
        GetAngularVelocity(eventPtr, out Vector3 angularVelocity);

        SetRotationAndPosition(angularVelocity, acceleration, orientation);
    }

    private void SetRotationAndPosition(Vector3 angularVelocity, Vector3 orientation, Vector3 acceleration)
    {
        // TODO : implement logic rotation here 
        _targetTransform.rotation *= Quaternion.Euler(angularVelocity * Time.deltaTime);
    }

    private void SetupJoyCon(SwitchControllerHID joyCon)
    {
        joyCon.SetLEDs(LEDStatusEnum.On);
        StartCoroutine(DelayedCalibration(joyCon, _timer));
        bool success = joyCon.SetIMUEnabled(true);
        if (!success)
        {
            Debug.LogWarning("Failed to enable IMU on Switch controller.");
        }
        _joyCon = joyCon;
    }

    private IEnumerator DelayedCalibration(SwitchControllerHID joyCon, float timer)
    {
        yield return new WaitForSeconds(timer);
        joyCon.CalibrateJoycon();
    }

    #endregion

    #region Getter

    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    private Vector3 GetOrientation(InputEventPtr eventPtr)
    {
        return _joyCon.orientation.ReadValueFromEvent(eventPtr);
    }
    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="orientation"></param>
    private void GetOrientation(InputEventPtr eventPtr, out Vector3 orientation)
    {
        orientation = _joyCon.orientation.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <returns></returns>
    private Vector3 GetAngularVelocity(InputEventPtr eventPtr)
    {
        return _joyCon.angularVelocity.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="angularVelocity"></param>
    private void GetAngularVelocity(InputEventPtr eventPtr, out Vector3 angularVelocity)
    {
        angularVelocity = _joyCon.angularVelocity.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <returns></returns>
    private Vector3 GetAcceleration(InputEventPtr eventPtr)
    {
        return _joyCon.acceleration.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="acceleration"></param>
    private void GetAcceleration(InputEventPtr eventPtr, out Vector3 acceleration)
    {
        acceleration = _joyCon.acceleration.ReadValueFromEvent(eventPtr);
    }
    #endregion
}