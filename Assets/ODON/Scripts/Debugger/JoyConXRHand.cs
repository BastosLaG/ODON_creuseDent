using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;

// * https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputDevice.html

public class JoyConXRHand : MonoBehaviour
{
    public bool isLeftHand = true;
    private InputDevice joyConLeft;
    private InputDevice joyConRight;

    [SerializeField] Transform targetTransform;

    [Range(0, 360)]
    [SerializeField] private float rotationValue = 90f;

    public float alpha = 0.70f; // 1 = 100% gyro, 0 = 100% accel
    private Quaternion orientation = Quaternion.identity;
    private Quaternion accelRotation = Quaternion.identity;
    private bool firstFrame = true;

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
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        {
            Debug.Log("Event is not a StateEvent or DeltaStateEvent, returning.");
            return;
        }

        // Lecture IMU
        InputDevice imuDevice = device;
        Vector3 gyro = Vector3.zero;
        Vector3 accel = Vector3.zero;

        if (isLeftHand && imuDevice is SwitchJoyConLHID left)
        {
            gyro = left.angularVelocity.ReadValueFromEvent(eventPtr);
            accel = left.acceleration.ReadValueFromEvent(eventPtr);
        }
        else if (!isLeftHand && imuDevice is SwitchJoyConRHID right)
        {
            gyro = right.angularVelocity.ReadValueFromEvent(eventPtr);
            accel = right.acceleration.ReadValueFromEvent(eventPtr);
        }

        if (accel.sqrMagnitude < 0.001f)
            return;

        Vector3 gravity = accel.normalized;

        float pitch = Mathf.Atan2(gravity.x, Mathf.Sqrt(gravity.y * gravity.y + gravity.z * gravity.z)) * Mathf.Rad2Deg;
        float roll = Mathf.Atan2(-gravity.y, -gravity.z) * Mathf.Rad2Deg;

        accelRotation = Quaternion.Euler(pitch, 0f, roll);

        Quaternion deltaRotation = Quaternion.Euler(gyro * Time.deltaTime);
        orientation = deltaRotation * orientation;

        if (!firstFrame)
        {
            orientation = Quaternion.Slerp(orientation, accelRotation, 1f - alpha);
        }
        else
        {
            orientation = accelRotation;
            firstFrame = false;
        }

        targetTransform.rotation = orientation;
    }
}