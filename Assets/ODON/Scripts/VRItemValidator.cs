using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

namespace ODON
{

    public class VRItemValidator : MonoBehaviour
    {
        private XRGrabInteractable grab;
        private UnityAction<SelectEnterEventArgs> onSelectEnterAction;

        [SerializeField] private UniversalSenderActionToEventManager uSATEManager;

        /////////////////////////////////////////////////////////////////////////////////

        #region Default Fonctions

        private void Awake()
        {
            onSelectEnterAction = (args) => uSATEManager.TryValdidateCurrentItem();
            if (uSATEManager == null)
            {
                Debug.LogError("UniversalSenderActionToEventManager is not assigned in VRPinceValidator.");
            }
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
        #endregion
    }
}