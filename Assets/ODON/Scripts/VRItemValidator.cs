using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

namespace ODON.Scripts
{
    
    public class VRItemValidator : MonoBehaviour
    {
        private XRGrabInteractable grab;
        private UnityAction<SelectEnterEventArgs> onSelectEnterAction;

        [SerializeField] private Data.SO_Step sO_Step; 

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
            bool success = GameManager.HighlightsManager.Instance.TryValidateCurrentItem(this.gameObject);
            if (success)
            {
                sO_Step.IsCorrect = true;
                sO_Step.ExecuteStep();
                sO_Step.IsCorrect = false;
            }
            else
            {
                sO_Step.IsCorrect = false;
                sO_Step.ExecuteStep();
                Debug.LogWarning($"Item {this.gameObject.name} validation failed.");
            }
        }

    }

}