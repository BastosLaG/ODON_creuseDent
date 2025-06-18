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
            bool success = GameManager.HighlightsManager.Instance.TryValidateCurrentItem(sO_Step);
            if (success)
            {
                GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(sO_Step.Id, true, sO_Step.Description);
            }
            else
            {
                GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(sO_Step.Id, false, sO_Step.Description);
            }
        }
    }
}