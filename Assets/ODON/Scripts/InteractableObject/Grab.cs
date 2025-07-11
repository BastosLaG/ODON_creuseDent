using UnityEngine;

namespace ODON.InteractableObject
{
    public class Grab : InteractAction
    {
        [SerializeField] private UniversalSenderActionToEventManager uSATEManager;
        public override void OnHeadInteract()
        {
            foreach (Data.Struct_VRValidatorObject item in uSATEManager.ValidatorObjects)
            {
                if (item.ValidateObject == transform.gameObject)
                {
                    item.IsValid = true;
                }
            }
            uSATEManager.TryValidateCurrentItem();

            SwapToHand();
        }
    }
}