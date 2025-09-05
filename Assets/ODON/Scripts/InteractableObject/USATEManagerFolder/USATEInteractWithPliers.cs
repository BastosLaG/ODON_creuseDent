using ODON.Data;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ODON.UsateManager
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class USATEInteractWithPliers : USATEInteract
    {
        [Header("Brewer Settings")]
        [SerializeField] private CapsuleCollider pliersCollider;
        [SerializeField] private Transform targetPosForGrab;
        [SerializeField] private GameObject targetObject;
        public GameObject TargetObject
        {
            get => targetObject;
            set => targetObject = value;
        }

        [Header("Hover")]
        [SerializeField] private Material hoverMaterial;

        [SerializeField] private Material defaultMaterial;
        private Renderer targetRenderer;

        private Transform targetPosParentReference;

        bool isPliserDam = false;

        protected new void Start()
        {
            base.Start();
            pliersCollider = GetComponent<CapsuleCollider>();
            if (pliersCollider == null)
            {
                Debug.LogError("No CapsuleCollider found on the object.", this);
            }
            pliersCollider.isTrigger = true;

            if (targetPosForGrab != null)
            {
                targetPosParentReference = targetPosForGrab.parent;
            }

            targetRenderer = targetObject.GetComponentInChildren<Renderer>();

            if (tag.CompareTo("PinceDigue") == 0)
            {
                // Debug.Log("The object is tagged as 'PinceDigue'.", this);
                isPliserDam = true;
            }
        }

        protected void Update()
        {
            if (debugInteractButton)
            {
                if (IsValidStep())
                {
                    interactInteractable.activated.Invoke(null);
                }
                debugInteractButton = false;
            }
        }

        ///////////////////////////////////////////////////////////////////
        #region Trigger System
        protected void OnTriggerEnter(Collider other)
        {
            if (isPliserDam && isOpen == true)
            {
                AddListenerToInteractable(other);
            }
            else if (!isPliserDam && isOpen == false)
            {
                AddListenerToInteractable(other);
            }
            else
            {
                // Debug.Log("Pliers state does not allow interaction.", this);
            }
        }
        protected void OnTriggerExit(Collider other)
        {
            if (isPliserDam && isOpen == true)
            {
                RemoveListenerToInteractable(other);
            }
            else if (!isPliserDam && isOpen == false)
            {
                RemoveListenerToInteractable(other);
            }
            else
            {
                Debug.Log("Pliers state does not allow interaction.", this);
            }
        }
        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Event Systeme
        private void AddListenerToInteractable(Collider other)
        {
            if (other.CompareTag(targetObject.tag))
            {
                SwapMaterialToHover(other);
                if (IsGoodTarget(other))
                {
                    if (interactInteractable != null)
                    {
                        interactInteractable.activated.RemoveListener(DoSomething);
                        interactInteractable.activated.AddListener(DoSomething);
                    }
                }
            }
            else
            {
                // Debug.Log($"{targetObject.tag} : {other.name} is not the target object.");
                // TODO : Implement non-blocking error handling
            }
        }
        private void RemoveListenerToInteractable(Collider other)
        {
            if (other.CompareTag(targetObject.tag))
            {
                SwapMaterialToDefault(other);
                if (IsGoodTarget(other))
                {
                    if (interactInteractable != null)
                    {
                        interactInteractable.activated.RemoveListener(DoSomething);
                    }
                }
            }
        }
        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Swap Material
        private void SwapMaterialToHover(Collider other)
        {
            other.transform.TryGetComponent<Renderer>(out targetRenderer);
            if (targetRenderer != null && hoverMaterial != null)
            {
                targetRenderer.sharedMaterial = hoverMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer or Hover Material is missing.", this);
            }
        }
        private void SwapMaterialToDefault(Collider other)
        {
            other.transform.TryGetComponent<Renderer>(out targetRenderer);
            if (targetRenderer != null && defaultMaterial != null)
            {
                targetRenderer.sharedMaterial = defaultMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer or Default Material is missing.", this);
            }
        }
        #endregion

        public bool IsGoodTarget(Collider other)
        {
            Debug.Log($" target tag : {other.CompareTag(targetObject.tag)} , GameObject : {other.gameObject == targetObject}");
            if (other.CompareTag(targetObject.tag) && other.gameObject == targetObject)
            {
                return true;
            }
            return false;
        }

        protected void DoSomething(ActivateEventArgs  args = null)
        {
            // TODO : Implement the desired functionality here
            if (isPliserDam)
            {
                if (targetObject.CompareTag(Tag.HoleDigue))
                {
                    // Digue the dam
                    GameManager.HighlightsTeethManager.Instance.OnDigDam.Invoke(true);
                    TryValidateCurrentItem();
                }
                else
                {
                    Debug.LogWarning($"No action defined for the tag {targetObject.tag}");
                    return;
                }
            }
            else
            {
                if (targetObject.CompareTag(Tag.Crampon))
                {
                    // Grab a specific object
                    if (!isOpen)
                    {
                        // Grab
                        targetObject.transform.parent = targetPosParentReference;
                        targetObject.transform.localPosition = Vector3.zero;
                        if (targetObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            rb.isKinematic = true;
                            rb.useGravity = false;
                        }
                        TryValidateCurrentItem();
                    }
                    else
                    {
                        // Drop
                        targetObject.transform.parent = targetPosParentReference;
                        targetObject.transform.localPosition = Vector3.zero;
                        if (targetObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            rb.useGravity = true;
                        }
                    }
                }
                else if (targetObject.CompareTag(Tag.Preview))
                {
                    // Place an specifics objects
                }
                else
                {
                    Debug.LogWarning($"No action defined for the tag {targetObject.tag}");
                    return;
                }
            }
        }
        
    }

}