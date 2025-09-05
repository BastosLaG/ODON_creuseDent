using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace ODON.UsateManager
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(BlendShapesDriver))]
    public abstract class USATEInteract : UniversalSenderActionToEventManager
    {
        [Header("Interact Settings")]
        [SerializeField] protected XRGrabInteractable interactInteractable;
        [SerializeField] protected BlendShapesDriver blendShapesDriver;
        [SerializeField] protected int rangeOfBlendShapesAction = 50;

        [Header("Debug Settings")]
        [SerializeField] protected bool isOpen = false;
        [SerializeField] protected bool debugInteractButton = false;

        protected new void Start()
        {
            base.Start();

            blendShapesDriver = GetComponent<BlendShapesDriver>();
            if (blendShapesDriver == null)
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
            if (isOpen)
            {
                ForceClose();
            }
            else
            {
                ForceOpen();
            }
        }

        public virtual void ForceClose()
        {
            isOpen = false;
            blendShapesDriver.GoToValue("Open", 0);
        }
        
        public virtual void ForceOpen()
        {
            isOpen = true;
            blendShapesDriver.GoToValue("Open", rangeOfBlendShapesAction);
        }
    }
}