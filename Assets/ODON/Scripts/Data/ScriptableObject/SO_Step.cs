using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// ScriptableObject representing a step in the ODON scenario.
    /// Stores the action ID and description, and provides methods to notify the scenario of success or failure.
    /// </summary>
    [CreateAssetMenu(fileName = "Step", menuName = "ODON/Step", order = 1)]
    public class SO_Step : ScriptableObject
    {
        /// <summary>
        /// The action ID for this step.
        /// </summary>
        [SerializeField] private E_NameActionInteractable id = E_NameActionInteractable.None;

        /// <summary>
        /// The description of this step.
        /// </summary>
        [SerializeField] private string description;

        /// <summary>
        /// Gets the action ID for this step.
        /// </summary>
        public E_NameActionInteractable Id => id;

        /// <summary>
        /// Gets the description of this step.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// Marks the action as passed and updates the scenario.
        /// </summary>
        public void ActionPassed()
        {
            GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(id, true, description);
        }

        /// <summary>
        /// Marks the action as failed and updates the scenario.
        /// </summary>
        public void ActionFailed()
        {
            GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(id, false, description);
        }
    }
}