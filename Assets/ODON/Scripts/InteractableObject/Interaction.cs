using UnityEngine;
using UnityEngine.Events;

namespace ODON.InteractableObject
{
    public class Interaction : InteractAction
    {
        [Header("Universal Sender Action To Event Manager")]
        [SerializeField] protected UniversalSenderActionToEventManager uSATEManager;

        [Header("Events")]
        public UnityEvent OnHeadInteractEvent;
        public UnityEvent OnHeadHoverEventBegin;
        public UnityEvent OnHeadHoverEventEnd;

        public override void HeadInteract()
        {
            OnHeadInteractEvent?.Invoke();
            SimpleGrabValidateCurrentItem();
            SwapToHand();
        }

        public override void HeadHoverEventBegin()
        {
            OnHeadHoverEventBegin?.Invoke();
        }

        public override void HeadHoverEventEnd()
        {
            OnHeadHoverEventEnd?.Invoke();
        }

        protected void SimpleGrabValidateCurrentItem()
        {
            foreach (Data.Struct_VRValidatorObject item in uSATEManager.ValidatorObjects)
            {
                if (item.ValidateObject == transform.gameObject)
                {
                    item.IsValid = true;
                }
            }
            uSATEManager.TryValidateCurrentItem();
        }
    }
}