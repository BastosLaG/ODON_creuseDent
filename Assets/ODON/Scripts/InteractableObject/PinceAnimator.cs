using UnityEngine;

namespace ODON.InteractableObject
{
    public class PinceAnimator : MonoBehaviour
    {
        [Header("Crampon Anchor")]
        [Tooltip("The anchor point for the crampon when attached to the pince.")]
        [SerializeField] private Transform cramponAnchor;
        [SerializeField] private Transform crampon = null;

        [Header("Animator")]
        [Tooltip("Animator component for controlling the pince animations.")]
        [SerializeField] private Animator animator;

        [SerializeField] private UniversalSenderActionToEventManager uSATEManagerTakeCrampon;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component is missing on the GameObject.");
            }
            PinceClose();
        }

        public void PinceOpen()
        {
            animator.SetBool("Close", false);
            animator.SetBool("Open", true);
            if (crampon != null)
            {
                AttachCrampon();
            }
        }
        public void PinceClose()
        {
            animator.SetBool("Close", true);
            animator.SetBool("Open", false);
            if (crampon != null)
            {
                DetachCrampon();
            }
        }

        private void AttachCrampon()
        {
            if (crampon != null)
            {
                crampon.GetComponent<Rigidbody>().isKinematic = true;
                Animation anim = crampon.GetComponent<Animation>();
                anim.Stop("Close");
                anim.Play("Open");

                crampon.SetParent(transform);
                crampon.SetPositionAndRotation(cramponAnchor.position, cramponAnchor.rotation);

                uSATEManagerTakeCrampon.TryValdidateCurrentItem();
            }
        }
        private void DetachCrampon()
        {
            crampon.SetParent(null);
            Animation anim = crampon.GetComponent<Animation>();
            anim.Stop("Open");
            anim.Play("Close");

            crampon.GetComponent<Rigidbody>().isKinematic = false;
            crampon = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Crampon"))
            {
                crampon = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Crampon"))
            {
                crampon = null;
            }
        }
    }
}