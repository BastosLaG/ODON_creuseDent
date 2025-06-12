using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class VRItemValidator : MonoBehaviour, ISendActiveCheckpointProgress
{
    public bool IsActiveCheckpointProgressEnabled { get; set; }
    public bool IsLocked { get; set; }

    private XRGrabInteractable grab;
    private UnityAction<SelectEnterEventArgs> onSelectEnterAction;

    void ISendActiveCheckpointProgress.SendActiveCheckpointProgress()
    {
        if (IsLocked) return;

        bool success = ODON.GameManager.HighlightsManager.Instance.TryValidateCurrentItem(this.gameObject);
        if (success)
        {
            IsActiveCheckpointProgressEnabled = true;
            Debug.Log($"Item {this.gameObject.name} validated successfully.");
        }
        else
        {
            IsActiveCheckpointProgressEnabled = false;
            Debug.LogWarning($"Item {this.gameObject.name} validation failed.");
        }
    }

    private void Awake()
    {
        onSelectEnterAction = (args) => OnGrabbed();
    }

    private void Start()
    {
        if (!TryGetComponent<XRGrabInteractable>(out grab))
        {
            Debug.Log($"XRGrabInteractable component not found. On {this.gameObject.name}.");
            grab = gameObject.AddComponent<XRGrabInteractable>();
        }

        grab.selectEntered.AddListener(onSelectEnterAction);
    }

    private void OnDisable()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(onSelectEnterAction);
        }
    }

    private void OnGrabbed()
    {
        ((ISendActiveCheckpointProgress)this).SendActiveCheckpointProgress();
    }
}
