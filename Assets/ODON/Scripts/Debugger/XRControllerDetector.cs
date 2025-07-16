using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class XRControllerDetector : MonoBehaviour
{
    void Start()
    {
        foreach (var device in InputSystem.devices)
        {
            // Debug.Log($"Detected device: {device.displayName} of type {device.GetType()}");
            if (device is UnityEngine.InputSystem.Switch.SwitchJoyConLHID controller)
            {
                Debug.Log($"Detected Left Controller: {controller.displayName}");
            }
            else if (device is UnityEngine.InputSystem.Switch.SwitchJoyConRHID rightController)
            {
                Debug.Log($"Detected Right Controller: {rightController.displayName}");
            }
        }
    }
}
