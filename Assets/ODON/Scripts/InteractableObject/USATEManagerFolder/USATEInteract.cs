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
            interactInteractable.activated.AddListener(SwitchBlendShapesAction);
        }

        void OnDisable()
        {
            interactInteractable.activated.RemoveListener(SwitchBlendShapesAction);
        }

        private void SwitchBlendShapesAction(ActivateEventArgs arg0)
        {
            if (!isOpen)
            {
                OpenPliers();
            }
            else
            {
                OpenPliers();
            }
            isOpen = !isOpen;
        }

        public void OpenPliers()
        {
            animator.SetBool("Close", false);
            animator.SetBool("Open", true);
        }
        public void ClosePliers()
        {
            animator.SetBool("Close", true);
            animator.SetBool("Open", false);
        }
    }
}