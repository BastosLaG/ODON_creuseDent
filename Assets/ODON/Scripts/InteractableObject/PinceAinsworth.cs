using UnityEngine;
using System.Collections.Generic;

namespace ODON.InteractableObject
{
    public class PinceAinsworth : MonoBehaviour
    {
        [Header("Preview Dig Dam Components")]
        [Tooltip("List of PreviewDigDam components in the scene.")]
        [SerializeField] private SphereCollider pinCollider;


        void Start()
        {
            pinCollider = GetComponent<SphereCollider>();
            if (pinCollider == null)
            {
                Debug.LogError("SphereCollider component is missing on the GameObject.");
            }
            Debug.Log($"PinCollider location: {pinCollider.transform.position}, Radius: {pinCollider.radius}");
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

    }
}