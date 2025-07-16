using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Controls;

// Todo - Détecter les Joy-Con gauche et droit
// Todo - Lire les controles des Joy con 
// Todo - Trouver un moyen d'émuler un clic OpenXR 
// Todo - émuler Une manette XR avec les joycons

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
                Debug.Log("Joy-Con gauche détecté !");
                LogControls(left);
                break;
            }
            else if (!isLeftHand && device is SwitchJoyConRHID right)
            {
                joyConRight = right;
                Debug.Log("Joy-Con droit détecté !");
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
