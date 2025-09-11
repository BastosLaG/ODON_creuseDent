using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace ODON.UsateManager
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(Animator))]
    public abstract class USATEInteract : UniversalSenderActionToEventManager
    {
        [Header("Interact Settings")]
        [SerializeField] protected XRGrabInteractable interactInteractable;
        [SerializeField] protected Animator animator;

        [Header("Debug Settings")]
        [SerializeField] protected bool isOpen = false;
        [SerializeField] protected bool debugInteractButton = false;

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

        void OnEnable()
        {
            interactInteractable.activated.AddListener(AnimatorShapesAction);
            interactInteractable.deactivated.AddListener(AnimatorShapesActionDeactivated);
        }


        void OnDisable()
        {
            interactInteractable.activated.RemoveListener(AnimatorShapesAction);
            interactInteractable.deactivated.RemoveListener(AnimatorShapesActionDeactivated);
        }


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
        private void AnimatorShapesActionDeactivated(DeactivateEventArgs arg0 = null)
        {
            AnimatorShapesAction();
        }
        public void SwitchAnimatorShapes()
        {
            AnimatorShapesAction();
        }

        private void OpenPliers()
        {
            animator.SetBool("Close", false);
            animator.SetBool("Open", true);
            isOpen = true;
        }
        private void ClosePliers()
        {
            animator.SetBool("Close", true);
            animator.SetBool("Open", false);
            isOpen = !true;
        }
    }
}