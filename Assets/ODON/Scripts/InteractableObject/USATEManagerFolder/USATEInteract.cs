using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ODON.UsateManager
{
    /// <summary>
    /// Abstract base class for USATE interactions.
    /// Handles animator and XRGrabInteractable setup, and manages pliers open/close logic.
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Animator))]
    public abstract class USATEInteract : UniversalSenderActionToEventManager
    {
        /// <summary>
        /// Reference to the XRGrabInteractable component.
        /// </summary>
        [Header("Interact Settings")]
        [SerializeField] protected XRGrabInteractable interactInteractable;

        /// <summary>
        /// Reference to the Animator component.
        /// </summary>
        [SerializeField] protected Animator animator;

        /// <summary>
        /// Indicates if the pliers are open.
        /// </summary>
        [Header("Debug Settings")]
        [SerializeField] protected bool isOpen = false;

        /// <summary>
        /// Enables debug interaction.
        /// </summary>
        [SerializeField] protected bool debugInteractButton = false;

        /// <summary>
        /// Initializes animator and interactable references.
        /// </summary>
        protected new void Start()
        {
            base.Start();

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator == null)
            {
                Debug.LogError("No BlendShapesDriver found on the object.", this);
            }
        }

        /// <summary>
        /// Registers animator event listeners when enabled.
        /// </summary>
        protected void OnEnable()
        {
            interactInteractable.activated.AddListener(AnimatorShapesAction);
            interactInteractable.deactivated.AddListener(AnimatorShapesActionDeactivated);
        }

        /// <summary>
        /// Unregisters animator event listeners when disabled.
        /// </summary>
        protected void OnDisable()
        {
            interactInteractable.activated.RemoveListener(AnimatorShapesAction);
            interactInteractable.deactivated.RemoveListener(AnimatorShapesActionDeactivated);
        }

        /// <summary>
        /// Handles activation event to switch pliers state.
        /// </summary>
        /// <param name="arg0">Optional activation event arguments.</param>
        private void AnimatorShapesAction(ActivateEventArgs arg0 = null)
        {
            if (!isOpen)
            {
                OpenPliers();
            }
            else
            {
                ClosePliers();
            }
        }

        /// <summary>
        /// Handles deactivation event to switch pliers state.
        /// </summary>
        /// <param name="arg0">Optional deactivation event arguments.</param>
        private void AnimatorShapesActionDeactivated(DeactivateEventArgs arg0 = null)
        {
            AnimatorShapesAction();
        }

        /// <summary>
        /// Switches the animator shapes (open/close).
        /// </summary>
        public void SwitchAnimatorShapes()
        {
            AnimatorShapesAction();
        }

        /// <summary>
        /// Opens the pliers by setting animator parameters.
        /// </summary>
        private void OpenPliers()
        {
            animator.SetBool("Close", false);
            animator.SetBool("Open", true);
            isOpen = true;
        }

        /// <summary>
        /// Closes the pliers by setting animator parameters.
        /// </summary>
        private void ClosePliers()
        {
            animator.SetBool("Close", true);
            animator.SetBool("Open", false);
            isOpen = !true;
        }
    }
}