using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

namespace ODON.UsateManager
{
    /// <summary>
    /// Handles grab interactions with custom events in the USATE system.
    /// Extends USATEGrab to invoke UnityEvents on grab actions and supports debug event triggering.
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    public class USATEGrabEvent : USATEGrab
    {
        /// <summary>
        /// Event invoked when the object is grabbed.
        /// </summary>
        [Header("Grab Event Settings")]
        [SerializeField] private UnityEvent OnGrabEvent;

        /// <summary>
        /// Enables debug event triggering.
        /// </summary>
        [SerializeField] private bool debugPlayEventButton;

        /// <summary>
        /// Initializes the component and base logic.
        /// </summary>
        protected new void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Handles debug event triggering and base update logic.
        /// </summary>
        protected new void Update()
        {
            if (debugPlayEventButton)
            {
                DoEvent();
            }
            base.Update();
        }

        /// <summary>
        /// Registers the select entered event listener and ensures correct event handling when enabled.
        /// </summary>
        protected new void OnEnable()
        {
            base.OnEnable();

            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
                grabInteractable.selectEntered.AddListener(OnSelectEntered);
            }
        }

        /// <summary>
        /// Unregisters the select entered event listener when disabled.
        /// </summary>
        protected new void OnDisable()
        {
            base.OnDisable();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            }
        }

        /// <summary>
        /// Handles the select entered event, triggers the custom event, and validates the step.
        /// </summary>
        /// <param name="args">Select enter event arguments.</param>
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            Debug.Log("Grab event triggered");
            DoEvent();

            base.OnSelectEntered(args);
        }

        /// <summary>
        /// Invokes the grab event if the step is valid and manages debug event state.
        /// </summary>
        private void DoEvent()
        {
            if (IsValidStep())
            {
                OnGrabEvent?.Invoke();
                debugPlayEventButton = true;
            }
            debugPlayEventButton = false;
        }
    }
}