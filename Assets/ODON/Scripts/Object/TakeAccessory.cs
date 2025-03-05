using UnityEngine;

public class TakeAccessory : MonoBehaviour
{

    [Header("Cible par défaut")]
    public GameObject target;
    public Transform localisedTarget;


    private bool isEquipped = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision détectée avec : " + other.name);

        if (LayerMask.NameToLayer("Object") == other.gameObject.layer)
        {
            Debug.Log("dans la main");
            AttachToTarget(target.transform, localisedTarget);
        }
    }

    private void AttachToTarget(Transform parent, Transform positionTarget)
    {
        Debug.Log("l'objet est bien attaché au joueur");
        transform.SetParent(parent);
        transform.localPosition = positionTarget.localPosition;
        transform.localRotation = Quaternion.identity;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        isEquipped = true;
    }
}