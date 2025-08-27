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
                debugPlayEventButton = false;
                OnGrabEvent?.Invoke();
                debugGrabButton = true;
            }
            base.Update();
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                // Debug.Log($"XRGrabInteractable found on {gameObject.name} we add OnGrabEventHandler");
                grabInteractable.selectEntered.AddListener(OnGrabEventHandler);
            }
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            if (grabInteractable != null)
            {
                // Debug.Log($"XRGrabInteractable found on {gameObject.name} we remove OnGrabEventHandler");
                grabInteractable.selectEntered.RemoveListener(OnGrabEventHandler);
            }
        }

        private void OnGrabEventHandler(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
        {
            OnGrabEvent?.Invoke();
        }
    }
}