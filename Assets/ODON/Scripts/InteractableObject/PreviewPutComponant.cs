using UnityEngine;

namespace ODON
{
    public class PreviewPutComponant : MonoBehaviour
    {
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material materialPreview;
        [SerializeField] private bool isContact = false;
        [SerializeField] private GameObject originalObject;
        [SerializeField] private Renderer rd;

        [SerializeField] private UniversalSenderActionToEventManager USATEManager;

        [SerializeField] private LayerMask layerToFind;

        void Start()
        {
            rd = GetComponent<Renderer>();
            rd.material = materialPreview;

            rd.enabled = false;
        }

        public void PlaceDigue()
        {
            if (isContact)
            {
                rd.enabled = true;
                rd.material = baseMaterial;
                Destroy(this);
                Destroy(originalObject);
                USATEManager.TryValdidateCurrentItem();
            }
            else
            {
                USATEManager.TryValdidateCurrentItem(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == layerToFind)
            {
                rd.enabled = true;
                isContact = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == layerToFind)
            {
                rd.enabled = false;
                isContact = false;
            }
        }
    }
}
