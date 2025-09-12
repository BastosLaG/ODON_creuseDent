using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.InteractableObject
{
    [Obsolete("PinceAinsworth is deprecated, use the new interaction system.")]
    public class PinceAinsworth : MonoBehaviour
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

    }
}