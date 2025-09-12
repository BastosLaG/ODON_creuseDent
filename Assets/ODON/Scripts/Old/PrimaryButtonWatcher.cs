using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class PrimaryButtonWatcher : MonoBehaviour
{
    public UnityEvent<bool> primaryButtonPress;

    private bool lastButtonState = false;
    private List<InputDevice> devicesWithPrimaryButton;

    private void Awake()
    {
        if (primaryButtonPress == null)
        {
            primaryButtonPress = new();
        }

        devicesWithPrimaryButton = new List<InputDevice>();
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithPrimaryButton.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool _))
        {
            devicesWithPrimaryButton.Add(device);
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWithPrimaryButton.Contains(device))
            devicesWithPrimaryButton.Remove(device);
    }

    void Update()
    {
        bool tempState = false;
        foreach (var device in devicesWithPrimaryButton)
        {
            tempState = device.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) // does action exist
                        && isPressed; // is action performed
        }

        if (tempState != lastButtonState) // Button state changed since last frame
        {
            primaryButtonPress.Invoke(tempState);
            lastButtonState = tempState;
        }
    }
}
