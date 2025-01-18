using UnityEngine;

public class TakeAccessory : MonoBehaviour
{
    [Header("Options d'équipement")]
    [Tooltip("Indique si l'objet à ramasser est une paire de gants")]
    public bool isGloves = false;
    public bool isInHands = false;
    
    [Header("Cible par défaut")]
    [Tooltip("Cible de l'accessoire pour un ramassage générique")]
    public GameObject target;
    [Tooltip("Position locale de l'objet par rapport à la cible par défaut")]
    public Transform localisedTarget;

    [Header("Équipement pour l'intérieur de la main")]
    [Tooltip("Cible de l'accessoire pour un ramassage générique")]
    public GameObject target_object;
    [Tooltip("Position locale de l'objet par rapport à la cible par défaut")]
    public Transform localisedTargetMiddleHands;

    [Header("Équipement spécifique pour les gants")]
    [Tooltip("Main gauche du joueur pour attacher le gant")]
    public GameObject leftHand;
    [Tooltip("Main droite du joueur pour attacher le gant")]
    public GameObject rightHand;
    
    [Tooltip("Préfabriqué du gant gauche à instancier sur la main gauche")]
    public GameObject leftglovePrefab;    
    [Tooltip("Préfabriqué du gant droit à instancier sur la main droite")]
    public GameObject rightglovePrefab;

    [Tooltip("Position cible pour le gant sur la main gauche")]
    public Transform leftHandTarget;
    [Tooltip("Position cible pour le gant sur la main droite")]
    public Transform rightHandTarget;


    [Header("Configuration des Meshes et Matériaux")]
    [SerializeField] private MeshRenderer rightHandMeshRenderer;
    [SerializeField] private MeshRenderer leftHandMeshRenderer;
    [SerializeField] private Material glovesMaterial;

    private bool isLeftHand = false;
    private bool isRightHand = false;
    private Material originalHandMaterial;

    private void OnTriggerEnter(Collider other) {
        
        if (!isGloves && other.tag == "Player") {
            HandlePickup(other);
        }
        else {
            if (other.name == "RightHand" && rightHandTarget != null) {
                EquipGlove(true, rightHandTarget);
                Debug.Log("Équipement ajouté à la main droite");
                isRightHand = true;

                if (isRightHand) {
                    Debug.Log("La main droite a bien ramassé cet accessoire.");
                }
            }
            else if (other.name == "LeftHand" && leftHandTarget != null) {
                EquipGlove(false, leftHandTarget);
                Debug.Log("Équipement ajouté à la main gauche");
                isLeftHand = true;

                if (isLeftHand) {
                    Debug.Log("La main gauche a bien ramassé cet accessoire.");
                }
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

    private void EquipGlove(bool isRightHand, Transform handTarget)
    {
        Debug.Log("Take accessory");
        // Crée un nouveau gant et le positionne sur la main cible sans déplacer la boîte
        GameObject glovePrefab = isRightHand ? rightglovePrefab : leftglovePrefab;
        GameObject glove = Instantiate(glovePrefab, handTarget);
        glove.transform.localPosition = Vector3.zero;
        glove.transform.localRotation = Quaternion.identity;

        if (isRightHand && rightHandMeshRenderer != null && glovesMaterial != null)
        {
            rightHandMeshRenderer.material = glovesMaterial;
            gameObject.SetActive(false);
        }
        else if (!isRightHand && leftHandMeshRenderer != null && glovesMaterial != null)
        {
            leftHandMeshRenderer.material = glovesMaterial;
            gameObject.SetActive(false);
        }
    }
}
