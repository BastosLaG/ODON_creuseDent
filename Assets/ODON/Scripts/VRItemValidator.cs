using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

namespace ODON
{

    public class VRItemValidator : MonoBehaviour
    {
        private XRGrabInteractable grab;
        [SerializeField] private UniversalSenderActionToEventManager uSATEManager;

        /////////////////////////////////////////////////////////////////////////////////

        #region Default Fonctions

        private void Awake()
        {
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

            grab.selectEntered.AddListener(OnGrabEntered);
        }

        private void OnDisable()
        {
            if (grab != null)
            {
                grab.selectEntered.RemoveListener(OnGrabEntered);
            }
        }

        private void OnGrabEntered(SelectEnterEventArgs args)
        {
            uSATEManager.TryValdidateCurrentItem();
        }
        
        #endregion
    }
}