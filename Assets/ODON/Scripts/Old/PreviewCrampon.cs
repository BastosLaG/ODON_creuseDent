using UnityEngine;
using System;


namespace ODON
{
    [Obsolete("PreviewCrampon is deprecated, use the new interaction system.")]
    public class CramponPreview : MonoBehaviour
    {
        [SerializeField] private Renderer rd;
        [SerializeField] private bool isValid = false;
        public bool IsValid => isValid;

        public void Init()
        {
            rd = GetComponentInChildren<Renderer>();
            rd.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Crampon"))
            {
                rd.enabled = true;
                isValid = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Crampon"))
            {
                rd.enabled = false;
                isValid = false;
            }
        }
    }
}