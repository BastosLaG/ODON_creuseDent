using ODON.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

namespace ODON.UsateManager
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class USATEInteractWithPliers : USATEInteract
    {
        #region Initialisation

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
        bool isGoodTarget = false;

        #endregion
        ///////////////////////////////////////////////////////////////////
        #region UnityFunction

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

        protected new void OnEnable()
        {
            base.OnEnable();
            interactInteractable.activated.AddListener(DoSomething);
        }
        protected new void OnDisable()
        {
            base.OnDisable();
            interactInteractable.activated.RemoveListener(DoSomething);
        }

        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Trigger System

        protected void OnTriggerEnter(Collider other)
        {
            SwapMaterialToHover(other);
            IsGoodTarget(other);
        }
        protected void OnTriggerExit(Collider other)
        {
            SwapMaterialToDefault(other);
            IsGoodTarget(other);
        }
        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Swap Material
        private void SwapMaterialToHover(Collider other)
        {
            if (targetObject.CompareTag(other.tag))
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
        }
        private void SwapMaterialToDefault(Collider other)
        {
            if (targetObject.CompareTag(other.tag))
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
        }
        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Boolean 
        public void IsGoodTarget(Collider other)
        {
            Debug.Log($" target tag : {other.CompareTag(targetObject.tag)} , GameObject : {other.gameObject == targetObject}");
            if (other.CompareTag(targetObject.tag) && other.gameObject == targetObject)
            {
                isGoodTarget = !isGoodTarget;
                Debug.Log($"Is good target set to : {isGoodTarget}");
            }
        }

        #endregion
        ///////////////////////////////////////////////////////////////////

        public void DoSomething(UnityEngine.XR.Interaction.Toolkit.ActivateEventArgs args = null)
        {
            if (isGoodTarget)
            {       
                if (isPliserDam && isOpen)
                {
                    // Digue the dam
                    if (targetObject.CompareTag(Tag.HoleDigue))
                    {
                        GameManager.HighlightsTeethManager.Instance.OnDigDam.Invoke(true);
                        TryValidateCurrentItem();
                    }
                    else
                    {
                        Debug.LogWarning($"No action defined for the tag {targetObject.tag}");
                        return;
                    }
                }
                else if (!isPliserDam && !isOpen)
                {
                    // Grab the crampon
                    if (targetObject.CompareTag(Tag.Crampon))
                    {
                        // Grab a specific object
                        if (!isOpen)
                        {
                            // Grab
                            // todo mettre dans des fonctions
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
                            // todo mettre dans des fonctions
                            targetObject.transform.parent = targetPosParentReference;
                            targetObject.transform.localPosition = Vector3.zero;
                            if (targetObject.TryGetComponent<Rigidbody>(out var rb))
                            {
                                rb.useGravity = true;
                            }
                        }
                    }
                    // 
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
                else
                {
                    Debug.Log("Try somthing else");
                }
            }
        }
    }

}