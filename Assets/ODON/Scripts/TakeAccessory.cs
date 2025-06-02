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
        }
        else if (isGloves)
        {
            if (other.name == "RightHand" && rightHandTarget != null)
            {
                EquipGlove(true, rightHandTarget);
            }
            else if (other.name == "LeftHand" && leftHandTarget != null)
            {
                EquipGlove(false, leftHandTarget);
            }
        }
    }

    private void AttachToTarget(Transform parent, Transform positionTarget)
    {
        
        transform.SetParent(parent);
        transform.localPosition = positionTarget.localPosition;
        transform.localRotation = Quaternion.identity;
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        isEquipped = true;
    }

    private void EquipGlove(bool isRightHand, Transform handTarget)
    {
       
        GameObject glovePrefab = isRightHand ? rightglovePrefab : leftglovePrefab;
        GameObject glove = Instantiate(glovePrefab, handTarget);
        glove.transform.localPosition = Vector3.zero;
        glove.transform.localRotation = Quaternion.identity;
        isEquipped = true;
    }
}