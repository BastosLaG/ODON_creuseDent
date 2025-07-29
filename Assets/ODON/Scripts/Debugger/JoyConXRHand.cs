using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;

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

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = transform;
        }

        joyConLeft = null;
        joyConRight = null;

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device: {device.displayName} - Type: {device.GetType()}");
            if (device is SwitchJoyConLHID left)
            {
                left.SetLEDs(LEDStatusEnum.On);
                left.SetIMUEnabled(true);
                joyConLeft = left;
                Debug.Log("Joy-Con left detect !");
            }
            else if (device is SwitchJoyConRHID right)
            {
                right.SetLEDs(LEDStatusEnum.On);
                right.SetIMUEnabled(true);
                joyConRight = right;
                Debug.Log("Joy-Con right detect !");
            }
        }

        if (joyConLeft != null)
        {
            foreach (var control in joyConLeft.allControls)
            {
                Debug.Log($"Control: {control.name}, Type: {control.GetType()}, ValueType: {control.valueType}");
            }
        }
        else if (joyConRight != null)
        {
            foreach (var control in joyConRight.allControls)
            {
                Debug.Log($"Control: {control.name}, Type: {control.GetType()}, ValueType: {control.valueType}");
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

    // private void OnInputEventReadGamePad(InputEventPtr eventPtr, InputDevice device)
    // {
    //     if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
    //         return;

    //     if (!(device is Gamepad gamepad))
    //         return;

    //     // Lire les valeurs
    //     Vector2 leftStick = gamepad.leftStick.ReadValueFromEvent(eventPtr);
    //     Vector2 rightStick = gamepad.rightStick.ReadValueFromEvent(eventPtr);
    //     float leftTrigger = gamepad.leftTrigger.ReadValueFromEvent(eventPtr);
    //     float rightTrigger = gamepad.rightTrigger.ReadValueFromEvent(eventPtr);

    //     bool buttonSouth = gamepad.buttonSouth.ReadValueFromEvent(eventPtr) > 0;
    //     bool buttonNorth = gamepad.buttonNorth.ReadValueFromEvent(eventPtr) > 0;
    //     bool buttonWest = gamepad.buttonWest.ReadValueFromEvent(eventPtr) > 0;
    //     bool buttonEast = gamepad.buttonEast.ReadValueFromEvent(eventPtr) > 0;

    //     bool dpadUp = gamepad.dpad.up.ReadValueFromEvent(eventPtr) > 0;
    //     bool dpadDown = gamepad.dpad.down.ReadValueFromEvent(eventPtr) > 0;
    //     bool dpadLeft = gamepad.dpad.left.ReadValueFromEvent(eventPtr) > 0;
    //     bool dpadRight = gamepad.dpad.right.ReadValueFromEvent(eventPtr) > 0;

    //     bool start = gamepad.startButton.ReadValueFromEvent(eventPtr) > 0;
    //     bool select = gamepad.selectButton.ReadValueFromEvent(eventPtr) > 0;
    //     bool leftShoulder = gamepad.leftShoulder.ReadValueFromEvent(eventPtr) > 0;
    //     bool rightShoulder = gamepad.rightShoulder.ReadValueFromEvent(eventPtr) > 0;

    //     // Debug (à utiliser avec précaution pour ne pas surcharger la console)
    //     Debug.Log($"Left Stick: {leftStick}, Right Stick: {rightStick}, Triggers: {leftTrigger}, {rightTrigger}");
    //     Debug.Log($"Buttons: A={buttonSouth}, B={buttonEast}, X={buttonWest}, Y={buttonNorth}");
    //     Debug.Log($"Dpad: Up={dpadUp}, Down={dpadDown}, Left={dpadLeft}, Right={dpadRight}");
    //     Debug.Log($"Start: {start}, Select: {select}, Left Shoulder: {leftShoulder}, Right Shoulder: {rightShoulder}");
    // }

    private void OnInputEventReadJoycon(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        {
            Debug.Log("Event is not a StateEvent or DeltaStateEvent, returning.");
            return;
        }

        if (device is not SwitchJoyConLHID && device is not SwitchJoyConRHID)
        {
            Debug.Log($"Device {device.displayName} is not a Joy-Con, returning.");
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