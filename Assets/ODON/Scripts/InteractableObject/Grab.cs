using UnityEngine;
using UnityEngine.Events;

namespace ODON.InteractableObject
{
    public class Grab : InteractAction
    {
        [SerializeField] protected UniversalSenderActionToEventManager uSATEManager;
        public UnityEvent OnHeadInteractEvent;
        public UnityEvent OnHandHoverEvent;

        public override void HeadInteract()
        {
            OnHeadInteractEvent?.Invoke();
            SimpleGrabValidateCurrentItem();
            SwapToHand();
        }

        public override void HeadHoverEventBegin()
        {
            OnHandHoverEvent?.Invoke();
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