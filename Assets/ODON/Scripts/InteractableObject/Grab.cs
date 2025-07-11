using UnityEngine;
using UnityEngine.Events;

namespace ODON.InteractableObject
{
    public class Grab : InteractAction
    {
        [SerializeField] protected UniversalSenderActionToEventManager uSATEManager;
        public UnityEvent OnHeadInteractEvent;

        public override void HeadInteract()
        {
            OnHeadInteractEvent?.Invoke();
            SimpleGrabValidateCurrentItem();
            SwapToHand();
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