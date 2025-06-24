using UnityEngine;
using System.Collections.Generic;

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
        private Animator animator;

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
                crampon.GetComponent<Animation>().Stop();
                crampon.SetParent(transform);
                crampon.SetPositionAndRotation(cramponAnchor.position, cramponAnchor.rotation);
                crampon.GetComponent<CramponPreview>().StartToCompareDistance();
            }
        }
        private void DetachCrampon()
        {
            if (crampon != null)
            {
                crampon.SetParent(null);
                crampon.GetComponent<Animation>().Play();
                crampon.GetComponent<Rigidbody>().isKinematic = false;
                crampon.GetComponent<CramponPreview>().PoseCrampon();
                crampon = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Crampon"))
            {
                crampon = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.name.Contains("Crampon"))
            {
                crampon = null;
            }
        }
    }
}