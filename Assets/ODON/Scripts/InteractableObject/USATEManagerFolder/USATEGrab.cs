using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace ODON.UsateManager
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class USATEGrab : UniversalSenderActionToEventManager
    {
        [Header("Grab Settings")]
        [SerializeField] protected XRGrabInteractable grabInteractable;
        [SerializeField] protected bool debugGrabButton;

        protected new void Start()
        {
            base.Start();
        }

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
        
        protected void OnEnable()
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(OnSelectEntered);
            }
        }

        protected void OnDisable()
        {
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            }
        }

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