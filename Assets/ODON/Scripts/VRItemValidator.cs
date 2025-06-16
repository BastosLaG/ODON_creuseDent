using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

namespace ODON
{
    
    public class VRItemValidator : MonoBehaviour
    {
        private XRGrabInteractable grab;
        private UnityAction<SelectEnterEventArgs> onSelectEnterAction;

        public int itemId;
        public bool IsActiveCheckpointProgressEnabled { get; set; }
        public bool IsLocked { get; set; }

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
            SendActiveCheckpointProgress();
        }

        void SendActiveCheckpointProgress()
        {
            if (IsLocked) return;

            bool success = GameManager.HighlightsManager.Instance.TryValidateCurrentItem(this.gameObject);
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

    }

}