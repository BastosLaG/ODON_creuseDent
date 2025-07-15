using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ODON.InteractableObject
{
    /// <summary>
    /// Handles interaction with an associated GameObject.
    /// This class extends the Interaction class to provide specific functionality for interacting with an associated object.
    /// </summary>
    /// <remarks>
    /// This class is used to manage interactions where the player can interact with an object that is associated with the current interactable object.
    /// It checks if the associated object is valid and performs the interaction logic accordingly.
    /// </remarks>
    public class InteractionWithAssociatedObject : Interaction
    {

        [Header("Associated Object")]
        [SerializeField] private GameObject associatedObject;
        [SerializeField] private GameObject combinedObject;

        /// <summary>
        /// Handles interaction with the associated object.
        /// This method checks if the associated object is valid and performs the interaction logic.
        /// It also validates the current item and checks if the hand is in a valid state for interaction.
        /// </summary>
        /// <remarks>
        /// This method is called when the player interacts with the object using their hand.
        /// It ensures that the associated object is set and that the hand is in a valid state
        /// before proceeding with the interaction.
        /// </remarks>
        public override void HeadInteract()
        {
            if (associatedObject != null)
            {
                Debug.Log($"Interacting with associated object: {associatedObject.name}");

                if (CheckHand())
                {
                    OnHeadInteractEvent?.Invoke();
                    SimpleGrabValidateCurrentItem();

                    // Todo - Implement the Logic here for interaction between the current object and the associated objects
                }
                else
                {
                    Debug.LogWarning("Hand is not in a valid state for interaction.");
                }
            }
            else
            {
                Debug.LogError("Associated object is not set.");
            }
        }

        /// <summary>
        /// Checks if the associated object is in the player's hand.
        /// </summary>
        /// <returns>True if the associated object is in the hand, false otherwise.</returns>
        /// <remarks>
        /// This method verifies if the associated object is either the left or right hand.
        /// It is used to ensure that the interaction is valid and that the object is being manipulated
        /// by the player's hand.
        /// </remarks>
        private bool CheckHand()
        {
            if (associatedObject == null)
            {
                Debug.LogError("Associated object is null.");
                return false;
            }

            if (associatedObject == GameManager.GameHandler.Instance.LeftHand.gameObject ||
                associatedObject == GameManager.GameHandler.Instance.RightHand.gameObject)
            {
                return true;
            }
            else
            {
                Debug.Log("Associated object is not in the hands.");
                return false;
            }
        }
    }
}