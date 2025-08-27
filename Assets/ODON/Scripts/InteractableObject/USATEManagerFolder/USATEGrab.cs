using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace ODON.UsateManager
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class USATEGrab : UniversalSenderActionToEventManager
    {
        [Header("Grab Settings")]
        [SerializeField] protected XRGrabInteractable grabInteractable;

        protected new void Start()
        {
            base.Start();
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

        private void OnSelectEntered(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
        {
            TryValidateCurrentItem();
        }

        public void TryValidateCurrentItem()
        {
            if (TargetHighlight.gameObject.GetComponent<Outline>().enabled == false) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
        }

        public void TryValidateCurrentItem(bool stepIsCorrect)
        {
            if (TargetHighlight.gameObject.GetComponent<Outline>().enabled == false) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
        }
    }
}