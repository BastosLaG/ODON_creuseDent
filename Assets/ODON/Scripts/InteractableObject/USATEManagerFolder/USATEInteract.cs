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

        protected void Update()
        {
            if (debugInteractButton)
            {
                if (isOpen)
                {
                    blendShapesDriver.GoToValue("Open", 100);
                }
                else
                {
                    blendShapesDriver.GoToValue("Open", 0);
                }
                debugInteractButton = false;
            }
        }
        
        protected virtual void OnEnable()
        {
            interactInteractable = GetComponent<XRGrabInteractable>();
            if (interactInteractable != null)
            {
                interactInteractable.activated.AddListener(OnActivedPressed);
            }
        }

        protected virtual void OnDisable()
        {
            if (interactInteractable != null)
            {
                interactInteractable.activated.RemoveListener(OnActivedPressed);
            }
        }

        private void OnActivedPressed(ActivateEventArgs args)
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                blendShapesDriver.GoToValue("Open", 100);
                // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
                if (!IsValidStep())
                {
                    Debug.Log("Le step actuel n'est pas le bon.");
                    return;
                }

                if (TryValidAction())
                {
                    TryValidateCurrentItem(); 
                }
            }
            else
            {
                blendShapesDriver.GoToValue("Open", 0);
            }
        }

        protected virtual bool TryValidAction()
        {
            // Default implementation, override in derived classes if needed
            return true;
        }
    }
}