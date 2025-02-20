using UnityEngine;

public class TakeAccessory : MonoBehaviour
{
    [Header("Options d'équipement")]
    public bool isGloves = false;

    [Header("Cible par défaut")]
    public GameObject target;
    public Transform localisedTarget;

    [Header("Équipement spécifique pour les gants")]
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject leftglovePrefab;
    public GameObject rightglovePrefab;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    private bool isEquipped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isEquipped) return; 

        if (!isGloves && other.CompareTag("Player"))
        {
            AttachToTarget(target.transform, localisedTarget);
            DestroyObject(localisedTarget);
        }
        else if (isGloves)
        {
            if (other.name == "RightHand" && rightHandTarget != null)
            {
                EquipGlove(true, rightHandTarget);
                DestroyObject(rightglovePrefab);
            }
            else if (other.name == "LeftHand" && leftHandTarget != null)
            {
                EquipGlove(false, leftHandTarget);
                DestroyObject(leftglovePrefab);

            }
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

    private void EquipGlove(bool isRightHand, Transform handTarget)
    {
        Debug.Log("le gant est bien attaché au joueur");
        GameObject glovePrefab = isRightHand ? rightglovePrefab : leftglovePrefab;
        GameObject glove = Instantiate(glovePrefab, handTarget);
        glove.transform.localPosition = Vector3.zero;
        glove.transform.localRotation = Quaternion.identity;
        isEquipped = true;
    }
}