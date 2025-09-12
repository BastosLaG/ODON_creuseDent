using JetBrains.Annotations;
using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// Serializable class representing a validator object for VR interactions in the ODON system.
    /// Stores references to the interactable object, its validation state, and the object to activate upon validation.
    /// </summary>
    [System.Serializable]
    public class Struct_VRValidatorObject
    {
        /// <summary>
        /// The interactable GameObject to be validated.
        /// </summary>
        [SerializeField] private GameObject ObjectInteractable;

        /// <summary>
        /// Indicates whether the object has been validated.
        /// </summary>
        [SerializeField] private bool isValid = false;

        /// <summary>
        /// The GameObject to activate when validation is successful.
        /// </summary>
        [SerializeField] private GameObject ObjectToActivateOnValid;

        /// <summary>
        /// Gets the interactable GameObject to be validated.
        /// </summary>
        public GameObject ValidateObject => ObjectInteractable;

        /// <summary>
        /// Gets or sets whether the object has been validated.
        /// </summary>
        public bool IsValid { get => isValid; set => isValid = value; }

        /// <summary>
        /// Gets the GameObject to activate when validation is successful.
        /// </summary>
        public GameObject ObjectToActivate => ObjectToActivateOnValid;
    }
}