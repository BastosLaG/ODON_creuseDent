using System;
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

        private Material defaultMaterial;
        private Renderer targetRenderer;

        private Transform targetPosParentReference;

        bool isPlisersDam = false;
        bool isGoodTarget = false;


        protected new void Start()
        {
            base.Start();
            pliersCollider = GetComponent<CapsuleCollider>();
            if (pliersCollider == null)
            {
                Debug.LogError("No CapsuleCollider found on the object.", this);
            }
            pliersCollider.isTrigger = true;

            if (targetPosParentReference != null)
            {
                targetPosParentReference = targetPosForGrab.parent;
            }
            targetRenderer = targetObject.GetComponent<Renderer>();
            defaultMaterial = targetRenderer.material;

            if (tag.CompareTo("PinceDigue") == 0)
            {
                Debug.Log("The object is tagged as 'PinceDigue'.", this);
                isPlisersDam = true;
            }
        }

        protected void Update()
        {
            if (debugInteractButton)
            {
                if (IsValidStep())
                {
                    Debug.Log("Interact triggered");
                    TryValidateCurrentItem();
                }
                debugInteractButton = false;
            }
        }

        ///////////////////////////////////////////////////////////////////
        #region Trigger System
        protected void OnTriggerEnter(Collider other)
        {
            if (isPlisersDam && isOpen == false)
            {
                AddListenerToInteractable(other);
            }
            else if (!isPlisersDam && isOpen == true)
            {
                AddListenerToInteractable(other);
            }
            else
            {
                Debug.Log("Pliers state does not allow interaction.", this);
            }
        }
        protected void OnTriggerExit(Collider other)
        {
            if (isPlisersDam && isOpen == false)
            {
                RemoveListenerToInteractable(other);
            }
            else if (!isPlisersDam && isOpen == true)
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
                        interactInteractable.activated.AddListener(DoSomething);
                    }
                }
            }
            else
            {
                Debug.Log($"{targetObject.tag} : {other.name} is not the target object.");
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
                isGoodTarget = false;
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
                defaultMaterial = targetRenderer.material;
                targetRenderer.material = hoverMaterial;
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
                targetRenderer.material = defaultMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer or Default Material is missing.", this);
            }
        }
        #endregion

        public bool IsGoodTarget(Collider other)
        {
            if (other.CompareTag(targetObject.tag) && other.gameObject == targetObject)
            {
                return true;
            }
            return false;
        }

        protected virtual void DoSomething(ActivateEventArgs args)
        {
            // TODO : Implement the desired functionality here
            if (isPlisersDam)
            {
                if (targetObject.CompareTag(Tag.HoleDigue))
                {
                    // Digue the dam
                    if (isGoodTarget)
                    {
                        GameManager.HighlightsTeethManager.Instance.OnDigDam.Invoke(true);
                    }
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
                    if (isOpen)
                    {
                        targetObject.transform.parent = targetPosForGrab;
                        targetObject.transform.localPosition = Vector3.zero;
                        if (targetObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            rb.isKinematic = true;
                            rb.useGravity = false;
                        }
                    }
                    else
                    {
                        targetObject.transform.parent = targetPosParentReference;
                        targetObject.transform.localPosition = Vector3.zero;
                        if (targetObject.TryGetComponent<Rigidbody>(out var rb))
                        {
                            rb.isKinematic = true;
                            rb.useGravity = false;
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