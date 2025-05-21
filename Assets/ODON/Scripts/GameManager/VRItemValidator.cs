using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRItemValidator : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent<XRGrabInteractable>(out var grab))
        {
            grab.selectEntered.AddListener((_) => OnGrabbed());
        }
        else
        {
            Debug.Log($"XRGrabInteractable component not found. On {this.gameObject.name}.");
            grab = gameObject.AddComponent<XRGrabInteractable>();
            grab.selectEntered.AddListener((_) => OnGrabbed());
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent<XRGrabInteractable>(out var grab))
        {
            grab.selectEntered.AddListener((_) => OnGrabbed());
        }
    }

    private void OnGrabbed()
    {
        bool success = ODON.GameHandler.Instance.TryValidateCurrentItem(this.gameObject);
        if (success)
        {
            ODON.GameHandler.Instance.SwitchActiveItem(1);
        }
    }

}
