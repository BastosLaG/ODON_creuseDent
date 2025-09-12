using UnityEngine;

namespace ODON.UsateManager
{
    /// <summary>
    /// Abstract base class for sending actions to the event manager in the USATE system.
    /// Handles step registration, highlighting, and validation logic.
    /// </summary>
    public abstract class UniversalSenderActionToEventManager : MonoBehaviour
    {
        /// <summary>
        /// The step associated with this interactable.
        /// </summary>
        [Header("Default Settings")]
        [SerializeField] protected Data.SO_Step step;

        /// <summary>
        /// Gets the step associated with this interactable.
        /// </summary>
        public Data.SO_Step Step => step;

        /// <summary>
        /// The transform used for highlighting in the UI.
        /// </summary>
        [SerializeField] protected Transform TargetHighlight = null;

        /// <summary>
        /// Initializes the highlight target and registers the step with the HighlightsManager.
        /// </summary>
        protected void Start()
        {
            if (TargetHighlight == null)
            {
                TargetHighlight = transform;
            }
            GameManager.HighlightsManager.Instance.RegisterStep(step, TargetHighlight);
        }

        /// <summary>
        /// Checks if the current step matches the required step.
        /// </summary>
        /// <returns>True if the step is valid, otherwise false.</returns>
        public bool IsValidStep()
        {
            Debug.Log($"IsValidStep called on {gameObject.name} for step {Step.name} and we need {GameManager.EventManager.Instance.CurrentStep} \n| Result: {Step == GameManager.EventManager.Instance.CurrentStep}");
            return Step == GameManager.EventManager.Instance.CurrentStep;
        }

        /// <summary>
        /// Attempts to validate the current item for the associated step.
        /// </summary>
        public virtual void TryValidateCurrentItem()
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
        }

        /// <summary>
        /// Attempts to validate the current item for the associated step, with a correctness flag.
        /// </summary>
        /// <param name="stepIsCorrect">Indicates if the step is correct.</param>
        public virtual void TryValidateCurrentItem(bool stepIsCorrect)
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
        }
    }
}