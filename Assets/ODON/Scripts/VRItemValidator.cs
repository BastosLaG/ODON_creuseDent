using UnityEngine.Events;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using ODON.Data;
using System.Collections.Generic;

namespace ODON.Scripts
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
            if(!uSATEManager.CheckIfValidatorObjectsAreValid()) return;

            if (uSATEManager.CheckIfStepIsActive())
            {
                SendActiveCheckpointProgress();
            }
            else
            {
                uSATEManager.Step.ActionFailed();
            }
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void SendActiveCheckpointProgress()
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(uSATEManager.Step);
        }
        #endregion
    }
}