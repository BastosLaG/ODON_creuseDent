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
        private bool isAllValid = false;
        [SerializeField] protected bool debugGrabMultipleButton;

        protected new void Start()
        {
            base.Start();
            foreach (var validator in validatorObjects)
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
            if (IsAllValid() && isAllValid == false)
            {
                TryValidateCurrentItem(true);
                isAllValid = true;
            }

            if (debugGrabMultipleButton)
            {
                if (IsValidStep())
                {
                    Debug.Log("Grab multiple triggered");
                    foreach (Struct_VRValidatorObject item in validatorObjects)
                    {
                        item.IsValid = true;
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
            if (vRValidatorObject.ObjectToActivate != null && vRValidatorObject.ValidateObject != null)
            {
                vRValidatorObject.ObjectToActivate.SetActive(true);
                vRValidatorObject.ValidateObject.SetActive(false);
                vRValidatorObject.IsValid = true;
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