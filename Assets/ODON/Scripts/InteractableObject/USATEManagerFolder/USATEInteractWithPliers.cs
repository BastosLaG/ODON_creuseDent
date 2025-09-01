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

        [Header("Hover")]
        [SerializeField] private Material hoverMaterial;

        private Material defaultMaterial;
        private Renderer targetRenderer;

        private Transform targetPosParentReference;


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
        }

        protected new void Update()
        {
            base.Update();
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

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetObject.tag) && other.gameObject == targetObject)
            {
                if (interactInteractable != null)
                {
                    interactInteractable.activated.AddListener(DoSomething);
                    if (targetRenderer != null && hoverMaterial != null)
                    {
                        targetRenderer.material = hoverMaterial;
                    }
                }
            }
            else if (other.CompareTag(targetObject.tag) && other.gameObject != targetObject)
            {
                Debug.Log($"{targetObject.tag} : {other.name} is not the target object.");
                // TODO : Implement non-blocking error handling
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetObject.tag) && other.gameObject == targetObject)
            {
                if (interactInteractable != null)
                {
                    interactInteractable.activated.RemoveListener(DoSomething);
                    if (targetRenderer != null && hoverMaterial != null)
                    {
                        targetRenderer.material = defaultMaterial;
                    }
                }
            }
        }

        protected virtual void DoSomething(ActivateEventArgs args)
        {
            // TODO : Implement the desired functionality here
            if (targetObject.CompareTag("Crampon"))
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
            else if (targetObject.CompareTag("Digue"))
            {
                // Digue the dam
            }
            else if (targetObject.CompareTag("Preview"))
            {
                // Place an specifics objects
            }
            else
            {
                Debug.LogWarning($"No action defined for the tag {targetObject.tag}");
                return;
            }
        }


        protected override bool TryValidAction()
        {
            return true;
        }
    }
}