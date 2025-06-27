using UnityEngine;

namespace ODON
{
    public class PreviewPutComponent : MonoBehaviour
    {
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private bool isInContact = false;
        [SerializeField] private GameObject originalObject;
        [SerializeField] private Renderer rendererComponent;
        [SerializeField] private UniversalSenderActionToEventManager eventManager;

        void Start()
        {
            rendererComponent = GetComponent<Renderer>();
            rendererComponent.material = previewMaterial;
            rendererComponent.enabled = false;
        }

        public void PlaceComponent()
        {
            if (isInContact)
            {
                rendererComponent.enabled = true;
                rendererComponent.material = baseMaterial;
                Destroy(this);
                Destroy(originalObject);
                eventManager.TryValidateCurrentItem();
            }
            else
            {
                eventManager.TryValidateCurrentItem(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == originalObject)
            {
                rendererComponent.enabled = true;
                isInContact = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == originalObject)
            {
                rendererComponent.enabled = false;
                isInContact = false;
            }
        }
    }
}
