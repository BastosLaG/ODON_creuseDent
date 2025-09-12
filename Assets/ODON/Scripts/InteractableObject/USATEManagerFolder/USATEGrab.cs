using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ODON.UsateManager
{
    /// <summary>
    /// Handles grab interactions in the USATE system.
    /// Manages XRGrabInteractable events, step validation, and item validation logic.
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    public class USATEGrab : UniversalSenderActionToEventManager
    {
        /// <summary>
        /// Reference to the XRGrabInteractable component.
        /// </summary>
        [Header("Grab Settings")]
        [SerializeField] protected XRGrabInteractable grabInteractable;

        /// <summary>
        /// Enables debug grab interaction.
        /// </summary>
        [SerializeField] protected bool debugGrabButton;

        /// <summary>
        /// Initializes the component and base logic.
        /// </summary>
        protected new void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Handles debug grab interaction and step validation.
        /// </summary>
        protected void Update()
        {
            if (debugGrabButton)
            {
                if (IsValidStep())
                {
                    grabInteractable.selectEntered.Invoke(null);
                }
                debugGrabButton = false;
            }
        }
        
        /// <summary>
        /// Registers the select entered event listener when enabled.
        /// </summary>
        protected void OnEnable()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(OnSelectEntered);
            }
        }

        /// <summary>
        /// Unregisters the select entered event listener when disabled.
        /// </summary>
        protected void OnDisable()
        {
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            }
        }

        /// <summary>
        /// Handles the select entered event, validates the step, and disables the script after validation.
        /// </summary>
        /// <param name="args">Select enter event arguments.</param>
        protected virtual void OnSelectEntered(SelectEnterEventArgs args)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            if (!IsValidStep())
            {
                Debug.Log("Le step actuel n'est pas le bon.");
                return;
            }

            TryValidateCurrentItem();
            this.enabled = false; // Désactive ce script pour éviter de valider plusieurs fois le même objet
        }
    }
}