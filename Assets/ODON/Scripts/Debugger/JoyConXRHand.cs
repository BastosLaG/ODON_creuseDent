using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;

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

    [SerializeField] private SwitchJoyConLHID _joyConLeft;
    [SerializeField] private SwitchJoyConRHID _joyConRight;

    [Header("Target Settings")]

    [Tooltip("The transform that will be rotated by Joy-Con input.")]
    [SerializeField] private Transform _targetTransform;
    private float angularVelocity_Z_Axis = 0;

    #region Primary Function
    private void Start()
    {
        if (_targetTransform == null)
        {
            _targetTransform = transform;
        }

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

        if (isLeftHand)
        {
            _joyConLeft?.SetLEDs(LEDStatusEnum.Flashing);
        }
        else
        {
            _joyConRight?.SetLEDs(LEDStatusEnum.Flashing);
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

        if (isLeftHand && device is SwitchJoyConLHID)
        {
            GetOrientation(eventPtr, _joyConLeft, out Vector3 orientation);
            GetAcceleration(eventPtr, _joyConLeft, out Vector3 acceleration);
            GetAngularVelocity(eventPtr, _joyConLeft, out Vector3 angularVelocity);
            // Debug.Log($"_joyConLeft = {_joyConLeft.name} est entrain de lire les informations suivantes : \nangularVelocity - {angularVelocity}");

            SetRotationAndPosition(angularVelocity, orientation, acceleration);
        }
        else if (!isLeftHand && device is SwitchJoyConRHID)
        {
            GetOrientation(eventPtr, _joyConRight, out Vector3 orientation);
            GetAcceleration(eventPtr, _joyConRight, out Vector3 acceleration);
            GetAngularVelocity(eventPtr, _joyConRight, out Vector3 angularVelocity);
            // Debug.Log($"_joyConRight = {_joyConRight.name} est entrain de lire les informations suivantes :\norientation - {orientation}\nacceleration - {acceleration}\nangularVelocity - {angularVelocity}");

            SetRotationAndPosition(angularVelocity, orientation, acceleration);
        }
    }

    private void SetRotationAndPosition(Vector3 angularVelocity, Vector3 orientation, Vector3 acceleration)
    {
        // Debug.Log($"{_targetTransform.name}");
        // TODO : implement logic rotation here 

        // refer to image "Image representing axes of rotation" 
        // * https://docs.google.com/document/d/10VK9m2KR3QEqI6O3c_fvIAioycqBt9m39Piq4ZZ_TYY/edit?tab=t.0 

        angularVelocity_Z_Axis += angularVelocity.z;

        Vector3 correctedRotation = new(-acceleration.x * 90, angularVelocity_Z_Axis, acceleration.y * 90);

        // Utiliser angularVelocity pour rotation relative

        Quaternion rotationTarget = Quaternion.Euler(correctedRotation);
        _targetTransform.localRotation = rotationTarget;
    }

    private void SetupJoyCon(SwitchJoyConLHID joyCon)
    {
        joyCon.SetLEDs(LEDStatusEnum.On);
        joyCon.StartRecordingCalibration();
        StartCoroutine(DelayedCalibration(joyCon));
        bool success = joyCon.SetIMUEnabled(true);
        if (!success)
        {
            Debug.LogWarning("Failed to enable IMU on Switch controller.");
        }
        _joyConLeft = joyCon;
    }
    private void SetupJoyCon(SwitchJoyConRHID joyCon)
    {
        joyCon.SetLEDs(LEDStatusEnum.On);
        joyCon.StartRecordingCalibration();
        StartCoroutine(DelayedCalibration(joyCon));
        bool success = joyCon.SetIMUEnabled(true);
        if (!success)
        {
            Debug.LogWarning("Failed to enable IMU on Switch controller.");
        }
        _joyConRight = joyCon;
    }

    private IEnumerator DelayedCalibration(SwitchJoyConRHID joyCon)
    {
        // Wait until calibration tools exist
        while (joyCon != null && joyCon.calibrationTools == null)
        {
            Debug.Log("Wait until calibration tools exist...");
            yield return null;
        }
        // Wait until buffer fills
        while (joyCon != null && joyCon.calibrationTools.GetThresholdSampleCount() < joyCon.calibrationTools.GetBufferSize())
        {
            Debug.Log($"{joyCon.name} : Threshold collect {joyCon.calibrationTools.GetThresholdSampleCount()} / {joyCon.calibrationTools.GetBufferSize()}");
            yield return null;
        }

        Debug.Log($"Try to calibrate joycon {joyCon.name}...");
        joyCon.CalibrateJoycon();
        Debug.Log($"Calibrate joycon complete {joyCon.name}");
    }
    private IEnumerator DelayedCalibration(SwitchJoyConLHID joyCon)
    {
        // Wait until calibration tools exist
        while (joyCon != null && joyCon.calibrationTools == null)
        {
            Debug.Log("Wait until calibration tools exist...");
            yield return null;
        }
        // Wait until buffer fills
        while (joyCon != null && joyCon.calibrationTools.GetThresholdSampleCount() < joyCon.calibrationTools.GetBufferSize())
        {
            Debug.Log($"{joyCon.name} : Threshold collect {joyCon.calibrationTools.GetThresholdSampleCount()} / {joyCon.calibrationTools.GetBufferSize()}");
            yield return null;
        }

        Debug.Log($"Try to calibrate joycon {joyCon.name}...");
        joyCon.CalibrateJoycon();
        Debug.Log($"Calibrate joycon complete {joyCon.name}");
    }
    #endregion

    #region Getter
    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="joycon"></param>
    /// <param name="orientation"></param>
    private void GetOrientation(InputEventPtr eventPtr, SwitchJoyConLHID joycon, out Vector3 orientation)
    {
        orientation = joycon.orientation.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the orientation vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="joycon"></param>
    /// <param name="orientation"></param>
    private void GetOrientation(InputEventPtr eventPtr, SwitchJoyConRHID joycon, out Vector3 orientation)
    {
        orientation = joycon.orientation.ReadValueFromEvent(eventPtr);
    }

    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="angularVelocity"></param>
    private void GetAngularVelocity(InputEventPtr eventPtr, SwitchJoyConLHID joycon, out Vector3 angularVelocity)
    {
        if (isLeftHand)
            angularVelocity = joycon.angularVelocity.ReadValueFromEvent(eventPtr);
        else
            angularVelocity = Vector3.zero;
    }
    /// <summary>
    /// Gets the angular velocity vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="angularVelocity"></param>
    private void GetAngularVelocity(InputEventPtr eventPtr, SwitchJoyConRHID joycon, out Vector3 angularVelocity)
    {
        if (!isLeftHand)
            angularVelocity = joycon.angularVelocity.ReadValueFromEvent(eventPtr);
        else
            angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="acceleration"></param>
    private void GetAcceleration(InputEventPtr eventPtr, SwitchJoyConLHID joycon, out Vector3 acceleration)
    {
        acceleration = joycon.acceleration.ReadValueFromEvent(eventPtr);
    }
    /// <summary>
    /// Gets the acceleration vector from the Joy-Con.
    /// </summary>
    /// <param name="eventPtr"></param>
    /// <param name="acceleration"></param>
    private void GetAcceleration(InputEventPtr eventPtr, SwitchJoyConRHID joycon, out Vector3 acceleration)
    {
        acceleration = joycon.acceleration.ReadValueFromEvent(eventPtr);
    }
    #endregion
}