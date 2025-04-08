using UnityEngine;

public class TakeAccessory : MonoBehaviour
{

    [Header("Cible par défaut")]
    [SerializeField] private GameObject target;
    [SerializeField] private Transform localisedTarget;
    [SerializeField] private Renderer[] brasRenderers;

    [SerializeField] private Material material_gloves;

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
            foreach (Renderer r in brasRenderers)
            {
                r.material = material_gloves;
            }

            // on dégage les gants
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