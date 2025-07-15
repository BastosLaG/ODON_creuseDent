using UnityEngine;

namespace ODON.InteractableObject
{
    public class InteractionWithAnimation : Interaction
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string animationName = "GrabAnimation"; 

        public override void HeadInteract()
        {
            OnHeadInteractEvent?.Invoke();
            SimpleGrabValidateCurrentItem();

            if (animator != null && !string.IsNullOrEmpty(animationName))
            {
                animator.SetTrigger(animationName);
            }
            else
            {
                Debug.LogWarning("Animator or animation name is missing.");
            }
        }
    }
}
