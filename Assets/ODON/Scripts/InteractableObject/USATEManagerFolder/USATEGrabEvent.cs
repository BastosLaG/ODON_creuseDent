using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;


namespace ODON.UsateManager
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class USATEGrabEvent : USATEGrab
    {
        [Header("Grab Event Settings")]
        [SerializeField] private UnityEvent OnGrabEvent;
        [SerializeField] private bool debugPlayEventButton; 

        protected new void Start()
        {
            base.Start();
        }

        protected new void Update()
        {
            if (debugPlayEventButton)
            {
                if (IsValidStep())
                {
                    Debug.Log("Grab event triggered");
                    OnGrabEvent?.Invoke();
                    debugGrabButton = true;
                }
                debugPlayEventButton = false;
            }
            base.Update();
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(OnGrabEventHandler);
            }
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnGrabEventHandler);
            }
        }

        private void OnGrabEventHandler(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            if (IsValidStep())
            {
                OnGrabEvent?.Invoke();
            }
        }
    }
}