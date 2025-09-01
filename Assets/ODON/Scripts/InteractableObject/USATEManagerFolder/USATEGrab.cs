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
                    Debug.Log("Grab triggered");
                    TryValidateCurrentItem();
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

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            if (!IsValidStep())
            {
                Debug.Log("Le step actuel n'est pas le bon.");
                return;
            }

            TryValidateCurrentItem();
        }
    }
}