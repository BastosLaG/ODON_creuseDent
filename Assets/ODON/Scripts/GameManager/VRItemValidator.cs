using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRItemValidator : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent<XRGrabInteractable>(out var grab))
        {
            grab.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent<XRGrabInteractable>(out var grab))
        {
            grab.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        GameObject grabbedObj = args.interactableObject.transform.gameObject;

        bool success = ODON.GameHandler.Instance.TryValidateCurrentItem(grabbedObj);
        if (success)
        {
            ODON.GameHandler.Instance.SwitchActiveItem(1);
        }
    }
}
