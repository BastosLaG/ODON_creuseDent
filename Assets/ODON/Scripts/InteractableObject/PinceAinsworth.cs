using UnityEngine;

namespace ODON.InteractableObject
{
    public class PinceAinsworth : MonoBehaviour, Interface.IInteractWithHeadInteractor
    {
        [Header("Preview Dig Dam Components")]
        [Tooltip("List of PreviewDigDam components in the scene.")]
        [SerializeField] private CapsuleCollider pinCollider;

        void Start()
        {
            pinCollider = GetComponent<CapsuleCollider>();
            if (pinCollider == null)
            {
                Debug.LogError("SphereCollider component is missing on the GameObject.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PreviewDigDam>() != null)
            {
                other.GetComponent<PreviewDigDam>().SetPreview(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PreviewDigDam>() != null)
            {
                other.GetComponent<PreviewDigDam>().SetPreview(false);
            }
        }

        public void OnHeadInteract()
        {
            // TODO : set pince action
            throw new System.NotImplementedException();
        }
    }
}