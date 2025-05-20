using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRItemValidator : MonoBehaviour
{
    [SerializeField] private ODON.Manager_outline outlineManager;

    private void OnEnable()
    {
        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnDisable()
    {
        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        GameObject grabbedObj = args.interactableObject.transform.gameObject;

        bool success = outlineManager.PoseSettings.TryValidateCurrentItem(grabbedObj);
        if (success)
        {
            outlineManager.EnableOutline();
        }
    }
}
