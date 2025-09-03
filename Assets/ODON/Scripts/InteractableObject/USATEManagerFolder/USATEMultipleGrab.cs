using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using ODON.Data;


namespace ODON.UsateManager
{
    public class USATEMultipleGrab : UniversalSenderActionToEventManager
    {
        [Header("Multiple GrabSettings")]
        [SerializeField] private Struct_VRValidatorObject[] validatorObjects;
        [SerializeField] protected bool debugGrabMultipleButton;

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

        private bool IsAllValid()
        {
            foreach (var validator in validatorObjects)
            {
                if (!validator.IsValid) return false;
            }
            return true;
        }

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
            this.enabled = false;

            if (IsAllValid())
            {
                TryValidateCurrentItem(true);
            }
        }

        public override void TryValidateCurrentItem()
        {
            if (!IsAllValid()) return;
            base.TryValidateCurrentItem();
        }

        public override void TryValidateCurrentItem(bool stepIsCorrect)
        {
            if (!IsAllValid()) return;
            base.TryValidateCurrentItem(stepIsCorrect);
        }
    }
}