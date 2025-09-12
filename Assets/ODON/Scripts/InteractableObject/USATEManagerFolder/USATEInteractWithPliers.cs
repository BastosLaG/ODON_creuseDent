using ODON.Data;
using UnityEngine;

namespace ODON.UsateManager
{
    /// <summary>
    /// Manages interactions with pliers in the USATE system.
    /// Handles grabbing, dropping, and dam actions based on the pliers type and target object.
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    public class USATEInteractWithPliers : USATEInteract
    {
        #region Initialisation

        /// <summary>
        /// Reference to the pliers' CapsuleCollider.
        /// </summary>
        [Header("Brewer Settings")]
        [SerializeField] private CapsuleCollider pliersCollider;

        /// <summary>
        /// Transform position for grabbing objects.
        /// </summary>
        [SerializeField] private Transform targetPosForGrab;

        /// <summary>
        /// The target object to interact with.
        /// </summary>
        [SerializeField] private GameObject targetObject;

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        public GameObject TargetObject
        {
            get => targetObject;
            set => targetObject = value;
        }

        /// <summary>
        /// Material used when hovering over objects.
        /// </summary>
        [Header("Hover")]
        [SerializeField] private Material hoverMaterial;

        /// <summary>
        /// Default material for objects.
        /// </summary>
        [SerializeField] private Material defaultMaterial;

        /// <summary>
        /// Renderer for the target object.
        /// </summary>
        private Renderer targetRenderer;

        /// <summary>
        /// Reference to the parent transform for grabbed objects.
        /// </summary>
        private Transform targetPosParentReference;

        /// <summary>
        /// Indicates if the pliers are for dam actions.
        /// </summary>
        bool isPliserDam = false;

        /// <summary>
        /// Indicates if the current target is valid.
        /// </summary>
        bool isGoodTarget = false;

        #endregion
        ///////////////////////////////////////////////////////////////////
        #region UnityFunction

        /// <summary>
        /// Initializes references and sets up the pliers.
        /// </summary>
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

        /// <summary>
        /// Handles debug interaction and step validation.
        /// </summary>
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

        /// <summary>
        /// Registers the DoSomething listener on enable.
        /// </summary>
        protected new void OnEnable()
        {
            base.OnEnable();
            interactInteractable.activated.AddListener(DoSomething);
        }

        /// <summary>
        /// Unregisters the DoSomething listener on disable.
        /// </summary>
        protected new void OnDisable()
        {
            base.OnDisable();
            interactInteractable.activated.RemoveListener(DoSomething);
        }

        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Trigger System

        /// <summary>
        /// Handles trigger enter events, swaps material and checks target validity.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        protected void OnTriggerEnter(Collider other)
        {
            SwapMaterialToHover(other);
            IsGoodTarget(other);
        }

        /// <summary>
        /// Handles trigger exit events, swaps material and checks target validity.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        protected void OnTriggerExit(Collider other)
        {
            SwapMaterialToDefault(other);
            IsGoodTarget(other);
        }
        #endregion
        ///////////////////////////////////////////////////////////////////
        #region Swap Material

        /// <summary>
        /// Swaps the material of the target object to the hover material.
        /// </summary>
        /// <param name="other">The collider to check and swap material.</param>
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

        /// <summary>
        /// Swaps the material of the target object to the default material.
        /// </summary>
        /// <param name="other">The collider to check and swap material.</param>
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

        /// <summary>
        /// Checks if the collider is the correct target and toggles the isGoodTarget flag.
        /// </summary>
        /// <param name="other">The collider to validate.</param>
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

        /// <summary>
        /// Executes the interaction logic based on pliers type and target object.
        /// Handles dam actions, grabbing, and dropping objects.
        /// </summary>
        /// <param name="args">Optional activation event arguments.</param>
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