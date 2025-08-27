using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using ODON.Data;
using UnityEditor.Callbacks;


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

        void Update()
        {
            if (IsAllValid())
            {
                TryValidateCurrentItem(true);
            }

            if (debugGrabMultipleButton)
            {
                debugGrabMultipleButton = false;
                foreach (Struct_VRValidatorObject item in validatorObjects)
                {
                    item.VRValidObject(true);
                }
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

        private void OnSelectEntered(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args, Struct_VRValidatorObject vRValidatorObject)
        {
            vRValidatorObject.VRValidObject(true);
        }

        public void TryValidateCurrentItem()
        {
            if (TargetHighlight.gameObject.GetComponent<Outline>().enabled == false) return;
            if (!IsAllValid()) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
        }

        public void TryValidateCurrentItem(bool stepIsCorrect)
        {
            if (TargetHighlight.gameObject.GetComponent<Outline>().enabled == false) return;
            if (!IsAllValid()) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
        }
    }
}