using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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
                DoEvent();
            }
            base.Update();
        }

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

        protected new void OnDisable()
        {
            base.OnDisable();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // TODO : Implémenter la gestion des erreurs bloquante et non bloquante
            Debug.Log("Grab event triggered");
            DoEvent();

            base.OnSelectEntered(args);
        }

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