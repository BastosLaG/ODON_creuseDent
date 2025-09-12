using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using ODON.Data;

namespace ODON.UsateManager
{
    /// <summary>
    /// Handles multiple grab interactions in the USATE system.
    /// Manages validation of multiple objects, step validation, and item activation logic.
    /// </summary>
    public class USATEMultipleGrab : UniversalSenderActionToEventManager
    {
        /// <summary>
        /// Array of validator objects to manage multiple grab validation.
        /// </summary>
        [Header("Multiple GrabSettings")]
        [SerializeField] private Struct_VRValidatorObject[] validatorObjects;

        /// <summary>
        /// Enables debug multiple grab interaction.
        /// </summary>
        [SerializeField] protected bool debugGrabMultipleButton;

        /// <summary>
        /// Initializes the component, sets up listeners, and configures rigidbodies.
        /// </summary>
        protected new void Start()
        {
            base.Start();
            foreach (Struct_VRValidatorObject validator in validatorObjects)
            {
                if (validator.ValidateObject.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
                {
                    grabInteractable.selectEntered.AddListener(args => OnSelectEntered(args, validator));
                    Rigidbody rb = validator.ValidateObject.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
                else
                {
                    Debug.LogWarning($"Validator object {validator.ValidateObject.name} does not have a XRGrabInteractable component.");
                }
            }
        }

        /// <summary>
        /// Removes listeners from grab interactables when disabled.
        /// </summary>
        private void OnDisable()
        {
            foreach (var validator in validatorObjects)
            {
                if (validator.ValidateObject.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
                {
                    grabInteractable.selectEntered.RemoveListener(args => OnSelectEntered(args, validator));
                }
            }
        }

        /// <summary>
        /// Handles debug multiple grab interaction and invokes select events for all validator objects.
        /// </summary>
        void Update()
        {
            if (debugGrabMultipleButton)
            {
                foreach (Struct_VRValidatorObject item in validatorObjects)
                {
                    if (item.ValidateObject.TryGetComponent<XRGrabInteractable>(out var grabInteractable))
                    {
                        grabInteractable.selectEntered.Invoke(new SelectEnterEventArgs());
                    }
                }
                debugGrabMultipleButton = false;
            }
        }

        /// <summary>
        /// Checks if all validator objects are valid.
        /// </summary>
        /// <returns>True if all objects are valid, otherwise false.</returns>
        private bool IsAllValid()
        {
            foreach (var validator in validatorObjects)
            {
                if (!validator.IsValid) return false;
            }
            return true;
        }

        /// <summary>
        /// Handles the select entered event for a validator object, activates and validates items.
        /// </summary>
        /// <param name="args">Select enter event arguments.</param>
        /// <param name="vRValidatorObject">The validator object being validated.</param>
        private void OnSelectEntered(SelectEnterEventArgs args, Struct_VRValidatorObject vRValidatorObject)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            if (!IsValidStep())
            {
                Debug.Log("Le step actuel n'est pas le bon.");
                return;
            }

            foreach (Struct_VRValidatorObject item in validatorObjects)
            {
                if (item.ValidateObject == vRValidatorObject.ValidateObject)
                {
                    item.IsValid = true;
                    item.ObjectToActivate.SetActive(true);
                    item.ValidateObject.SetActive(false);
                }
            }
            enabled = false;

            if (IsAllValid())
            {
                TryValidateCurrentItem(true);
            }
        }

        /// <summary>
        /// Attempts to validate the current item for the associated step if all objects are valid.
        /// </summary>
        public override void TryValidateCurrentItem()
        {
            if (!IsAllValid()) return;
            base.TryValidateCurrentItem();
        }

        /// <summary>
        /// Attempts to validate the current item for the associated step, with a correctness flag, if all objects are valid.
        /// </summary>
        /// <param name="stepIsCorrect">Indicates if the step is correct.</param>
        public override void TryValidateCurrentItem(bool stepIsCorrect)
        {
            if (!IsAllValid()) return;
            base.TryValidateCurrentItem(stepIsCorrect);
        }
    }
}