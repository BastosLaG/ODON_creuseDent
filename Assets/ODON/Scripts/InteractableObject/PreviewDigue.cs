using UnityEditor.SceneManagement;
using UnityEngine;

namespace ODON
{
    public class DiguePreview : MonoBehaviour
    {
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Material materialPreview;
        [SerializeField] private bool isContact = false;

        [SerializeField] private Renderer rd;

        void Start()
        {
            rd = GetComponent<Renderer>();
            rd.material = materialPreview;

            rd.enabled = false;
        }

        void PlaceDigue()
        {
            if (isContact)
            {
                rd.enabled = true;
                rd.material = baseMaterial;
                Destroy(this);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Dam"))
            {
                rd.enabled = true;
                isContact = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Dam"))
            {
                rd.enabled = false;
                isContact = false;
            }
        }
    }
}
