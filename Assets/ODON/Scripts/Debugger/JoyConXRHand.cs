using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;

// Todo - Detecter les Joy-Con gauche et droit          V
// Todo - Lire les controls des Joy con                 X
// Todo - Find a method to generate a click OpenXR      X
// Todo - Create an interface XR between the Joy-cons   X

// * https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputDevice.html

public class JoyConXRHand : MonoBehaviour
{
    public bool isLeftHand = true;
    private InputDevice joyConLeft;
    private InputDevice joyConRight;

    void Start()
    {
        joyConLeft = null;
        joyConRight = null;

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device: {device.displayName} - Type: {device.GetType()}");
            if (isLeftHand && device is SwitchJoyConLHID left)
            {
                joyConLeft = left;
                Debug.Log("Joy-Con left detect !");
                LogControls(left);
                break;
            }
            else if (!isLeftHand && device is SwitchJoyConRHID right)
            {
                joyConRight = right;
                Debug.Log("Joy-Con right detect !");
                LogControls(right);
                break;
            }
        }
    }

    void LogControls(InputDevice device)
    {
        Debug.Log($"Controls for {device.displayName}:");
        foreach (var control in device.allControls)
        {
            Debug.Log($" - {control.name} ({control.displayName}) type: {control.GetType()}");
        }
    }

    void Update()
    {

    }
}
