using UnityEngine;
using UnityEngine.InputSystem;

public class HIDDebugger : MonoBehaviour
{
    void Start()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device: {device.displayName} | Type: {device.GetType()}");
        }
    }
}
