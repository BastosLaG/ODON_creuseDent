using UnityEngine;

public class TakeAccessory : MonoBehaviour
{

    [Header("Cible par défaut")]
    public GameObject target;
    public Transform localisedTarget;

    public Material material_gloves;

    public bool isGloves;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision détectée avec : " + other.name);

        if (LayerMask.NameToLayer("Object") == other.gameObject.layer)
        {
            Debug.Log("dans la main");
            AttachToTarget(target.transform, localisedTarget, other);
        }
    }

    private void AttachToTarget(Transform parent, Transform positionTarget, Collider other)
    {
        Debug.Log("l'objet est bien attaché au joueur");
        
        if (isGloves)
        {
            other.GetComponent<Renderer>().material = material_gloves;
            Renderer handRenderer = parent.GetComponent<Renderer>();

            if (handRenderer != null)
            {
                // Change le matériau des mains par celui des gants
                handRenderer.material = GetComponent<Renderer>().material;
            }

            // On enlève les gants
            gameObject.SetActive(false);
        }
        else
        {
            transform.SetParent(parent);
            transform.position = parent.position;
            transform.rotation = parent.rotation;//pour la blouse
        }
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
    }
}