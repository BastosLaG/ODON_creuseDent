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

        /////////////////////////////////////////////////////////////////////////////////

        #region Default Fonctions

        private void Awake()
        {
            onSelectEnterAction = (args) => OnGrabbed();
        }

        private void Start()
        {
            GameManager.HighlightsManager.Instance.RegisterStep(sO_Step, this);

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
        #endregion

        /////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void SendActiveCheckpointProgress()
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(sO_Step);
        }
        
        #endregion
    }
}