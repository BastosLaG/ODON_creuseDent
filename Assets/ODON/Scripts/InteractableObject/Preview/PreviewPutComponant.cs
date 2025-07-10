using UnityEngine;

namespace ODON.InteractableObject
{
    public class PreviewPutComponent : MonoBehaviour, Interface.IInteractWithHeadInteractor
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
                eventManager.TryValidateCurrentItem();
                Destroy(originalObject);
                Destroy(this);
            }
            else
            {
                eventManager.TryValidateCurrentItem(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log($"_____________________________________________________________");
            Debug.Log($"{other.gameObject.layer} try to enter in {gameObject.layer} and the good object is {originalObject.layer}");
            Debug.Log($"_____________________________________________________________");
            if (other.gameObject.layer == originalObject.layer)
            {
                rendererComponent.enabled = true;
                isInContact = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == originalObject.layer)
            {
                rendererComponent.enabled = false;
                isInContact = false;
            }
        }

        public void OnHeadInteract()
        {
            // TODO : Set Preview
            throw new System.NotImplementedException();
        }
    }
}
