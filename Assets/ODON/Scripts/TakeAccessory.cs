using UnityEngine;

public class TakeAccessory : MonoBehaviour
{
    [Header("Options d'équipement")]
    [Tooltip("Indique si l'objet à ramasser est une paire de gants")]
    public bool isGloves = false;
    
    [Header("Cible par défaut")]
    [Tooltip("Cible de l'accessoire pour un ramassage générique")]
    public GameObject target;
    [Tooltip("Position locale de l'objet par rapport à la cible par défaut")]
    public Transform localisedTarget;

    [Header("Équipement spécifique pour les mains")]
    [Tooltip("Main gauche du joueur pour attacher le gant")]
    public GameObject leftHand;
    [Tooltip("Main droite du joueur pour attacher le gant")]
    public GameObject rightHand;
    
    [Tooltip("Préfabriqué du gant à instancier sur la main")]
    public GameObject glovePrefab;
    
    [Tooltip("Position cible pour le gant sur la main gauche")]
    public Transform leftHandTarget;
    [Tooltip("Position cible pour le gant sur la main droite")]
    public Transform rightHandTarget;

    private bool isLeftHand = false;
    private bool isRightHand = false;

    private void OnTriggerEnter(Collider other)
    {
        // If the object is not a glove box, proceed with generic pickup
        if (!isGloves && other.tag == "Player")
        {
            HandlePickup(other);
        }
        else
        {
            // Vérifie si c'est la main droite ou gauche qui entre en collision
            if (other.name == "RightHand" && rightHandTarget != null)
            {
                EquipGlove(rightHandTarget);
                Debug.Log("Equipement ajouté à la main droite");
                isRightHand = true;
            }
            else if (other.name == "LeftHand" && leftHandTarget != null)
            {
                EquipGlove(leftHandTarget);
                Debug.Log("Equipement ajouté à la main gauche");
                isLeftHand = true;
            }
        }
    }

    private void HandlePickup(Collider other, Transform customTarget = null)
    {
        Debug.Log("Take accessory");
        // Set the parent of this object to the target (or custom target if provided)
        transform.SetParent(customTarget != null ? customTarget : target.transform);
        
        // Set the local position to the specified localisedTarget or custom target
        transform.localPosition = customTarget != null ? customTarget.localPosition : localisedTarget.localPosition;

        // Destroy the Rigidbody and Collider components on this object, if they exist
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider bc = GetComponent<Collider>();
        if (rb != null) Destroy(rb);
        if (bc != null) Destroy(bc);
    }

    private void EquipGlove(Transform handTarget)
    {
        Debug.Log("Take accessory");
        // Crée un nouveau gant et le positionne sur la main cible sans déplacer la boîte
        GameObject glove = Instantiate(glovePrefab, handTarget);
        glove.transform.localPosition = Vector3.zero;
        glove.transform.localRotation = Quaternion.identity;
    }
}
