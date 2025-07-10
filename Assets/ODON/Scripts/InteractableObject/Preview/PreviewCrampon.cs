using UnityEngine;

namespace ODON.InteractableObject
{
    public class CramponPreview : MonoBehaviour, Interface.IInteractWithHeadInteractor
    {
        [SerializeField] private Renderer rd;
        [SerializeField] private bool isValid = false;
        public bool IsValid => isValid;

        public void Init()
        {
            rd = GetComponentInChildren<Renderer>();
            rd.enabled = false;
        }

        public void OnHeadInteract()
        {
            // TODO : Set Crampon on mouth patient
            throw new System.NotImplementedException();
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