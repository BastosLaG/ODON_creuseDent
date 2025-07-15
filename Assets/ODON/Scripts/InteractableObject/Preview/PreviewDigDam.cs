using System;
using UnityEngine;
using UnityEngine.Android;

namespace ODON.InteractableObject
{
    public class PreviewDigDam : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        public MeshRenderer MR => meshRenderer;
        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("MeshRenderer component is missing on the GameObject.");
            }

            // Initially disable the mesh renderer
            meshRenderer.enabled = false; 
        }

        public void SetPreview(bool isActive)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = isActive;
            }
        }
    }    
}