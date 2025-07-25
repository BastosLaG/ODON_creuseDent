using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.LowLevel;

// Todo - Detecter les Joy-Con gauche et droit          V
// Todo - Lire les controls des Joy con                 V
// Todo - Find a method to generate a click OpenXR      V
// Todo - Create an interface XR between the Joy-cons   X

// * https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputDevice.html

public class JoyConXRHand : MonoBehaviour
{
    public bool isLeftHand = true;
    private InputDevice joyConLeft;
    private InputDevice joyConRight;

    [SerializeField] Transform targetTransform;

    [Range(0, 360)]
    [SerializeField] private float rotationValue = 90f;

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

        if (isLeftHand && device is SwitchJoyConLHID joyconLeft)
        {
            float smooth = 5.0f;

            // Debug.Log("Joy-Con Left detected");
            // Lire les valeurs
            Vector3 angularVelocity = joyconLeft.angularVelocity.ReadValueFromEvent(eventPtr);
            Vector3 orientation = joyconLeft.orientation.ReadValueFromEvent(eventPtr);
            Vector3 acceleration = joyconLeft.acceleration.ReadValueFromEvent(eventPtr);

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(
                acceleration.x * rotationValue,
                0,
                acceleration.z * rotationValue
            );
            targetTransform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
            // Debug.Log($"Angular Velocity: {angularVelocity}, Orientation: {orientation}, Acceleration: {acceleration}");

            // Vector2 leftStick = joyconLeft.leftStick.ReadValueFromEvent(eventPtr);
            // Vector2 rightStick = joyconLeft.rightStick.ReadValueFromEvent(eventPtr);
            // float leftTrigger = joyconLeft.leftTrigger.ReadValueFromEvent(eventPtr);
            // float rightTrigger = joyconLeft.rightTrigger.ReadValueFromEvent(eventPtr);

            // bool buttonSouth = joyconLeft.buttonSouth.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonNorth = joyconLeft.buttonNorth.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonWest = joyconLeft.buttonWest.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonEast = joyconLeft.buttonEast.ReadValueFromEvent(eventPtr) > 0;

            // bool dpadUp = joyconLeft.dpad.up.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadDown = joyconLeft.dpad.down.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadLeft = joyconLeft.dpad.left.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadRight = joyconLeft.dpad.right.ReadValueFromEvent(eventPtr) > 0;

            // bool leftShoulder = joyconLeft.leftShoulder.ReadValueFromEvent(eventPtr) > 0;
            // bool leftShoulderMini = joyconLeft.leftShoulderMini.ReadValueFromEvent(eventPtr) > 0;
            // bool rightShoulder = joyconLeft.rightShoulder.ReadValueFromEvent(eventPtr) > 0;
            // bool rightShoulderMini = joyconLeft.rightShoulderMini.ReadValueFromEvent(eventPtr) > 0;

            // // Debug (à utiliser avec précaution pour ne pas surcharger la console)
            // Debug.Log($"Left Stick: {leftStick}, Right Stick: {rightStick}, Triggers: {leftTrigger}, {rightTrigger}");
            // Debug.Log($"Buttons: A={buttonSouth}, B={buttonEast}, X={buttonWest}, Y={buttonNorth}");
            // Debug.Log($"Dpad: Up={dpadUp}, Down={dpadDown}, Left={dpadLeft}, Right={dpadRight}");
            // Debug.Log($"Left Shoulder: {leftShoulder}, Left Shoulder Mini: {leftShoulderMini}, Right Shoulder: {rightShoulder}, Right Shoulder Mini: {rightShoulderMini}");
        }
        else if (!isLeftHand && device is SwitchJoyConRHID joyconRight)
        {
            float smooth = 5.0f;

            // Debug.Log("Joy-Con Left detected");
            // Lire les valeurs
            Vector3 angularVelocity = joyconRight.angularVelocity.ReadValueFromEvent(eventPtr);
            Vector3 orientation = joyconRight.orientation.ReadValueFromEvent(eventPtr);
            Vector3 acceleration = joyconRight.acceleration.ReadValueFromEvent(eventPtr);

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(
                acceleration.x * rotationValue,
                0,
                acceleration.z * rotationValue
            );

            targetTransform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
            // Debug.Log($"Angular Velocity: {angularVelocity}, Orientation: {orientation}, Acceleration: {acceleration}");

            // Vector2 leftStick = joyconRight.leftStick.ReadValueFromEvent(eventPtr);
            // Vector2 rightStick = joyconRight.rightStick.ReadValueFromEvent(eventPtr);
            // float leftTrigger = joyconRight.leftTrigger.ReadValueFromEvent(eventPtr);
            // float rightTrigger = joyconRight.rightTrigger.ReadValueFromEvent(eventPtr);

            // bool buttonSouth = joyconRight.buttonSouth.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonNorth = joyconRight.buttonNorth.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonWest = joyconRight.buttonWest.ReadValueFromEvent(eventPtr) > 0;
            // bool buttonEast = joyconRight.buttonEast.ReadValueFromEvent(eventPtr) > 0;

            // bool dpadUp = joyconRight.dpad.up.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadDown = joyconRight.dpad.down.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadLeft = joyconRight.dpad.left.ReadValueFromEvent(eventPtr) > 0;
            // bool dpadRight = joyconRight.dpad.right.ReadValueFromEvent(eventPtr) > 0;

            // bool leftShoulder = joyconRight.leftShoulder.ReadValueFromEvent(eventPtr) > 0;
            // bool leftShoulderMini = joyconRight.leftShoulderMini.ReadValueFromEvent(eventPtr) > 0;
            // bool rightShoulder = joyconRight.rightShoulder.ReadValueFromEvent(eventPtr) > 0;
            // bool rightShoulderMini = joyconRight.rightShoulderMini.ReadValueFromEvent(eventPtr) > 0;

            // // Debug (à utiliser avec précaution pour ne pas surcharger la console)
            // Debug.Log($"Left Stick: {leftStick}, Right Stick: {rightStick}, Triggers: {leftTrigger}, {rightTrigger}");
            // Debug.Log($"Buttons: A={buttonSouth}, B={buttonEast}, X={buttonWest}, Y={buttonNorth}");
            // Debug.Log($"Dpad: Up={dpadUp}, Down={dpadDown}, Left={dpadLeft}, Right={dpadRight}");
            // Debug.Log($"Left Shoulder: {leftShoulder}, Left Shoulder Mini: {leftShoulderMini}, Right Shoulder: {rightShoulder}, Right Shoulder Mini: {rightShoulderMini}");
        }
    }

}
