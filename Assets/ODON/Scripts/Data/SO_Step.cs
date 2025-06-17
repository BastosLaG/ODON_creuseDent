using UnityEngine;

namespace ODON.Data
{

    [CreateAssetMenu(fileName = "Step", menuName = "ODON/Step", order = 1)]
    public class SO_Step : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string description;
        [SerializeField] private bool isActive = true;

        public int Id => id;
        public string Description => description;
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        private void OnEnable()
        {
            if (Application.isPlaying && GameManager.EventManager.Instance != null)
            {
                GameManager.EventManager.Instance.OnActionPassed += OnActionPassedHandler;
                GameManager.EventManager.Instance.OnActionNotPassed += OnActionNotPassedHandler;
                GameManager.EventManager.Instance.OnActionAlreadyPassed += OnActionAlreadyPassedHandler;
            }
        }

        private void OnDestroy()
        {
            ResetStep();
            if (Application.isPlaying && GameManager.EventManager.Instance != null)
            {
                GameManager.EventManager.Instance.OnActionPassed -= OnActionPassedHandler;
                GameManager.EventManager.Instance.OnActionNotPassed -= OnActionNotPassedHandler;
                GameManager.EventManager.Instance.OnActionAlreadyPassed -= OnActionAlreadyPassedHandler;
            }
        }

        public void ResetStep()
        {
            isActive = true;
        }


        /// <summary>
        /// Executes the step if the action is passed correctly.
        /// </summary>
        /// <remarks>
        /// This method is called when an action is passed correctly. It checks if the step is active and if the ID and description match. And check if this is the good step to execute.
        /// </remarks>
        /// <param name="stepId">The ID of the step that was passed.</param>
        /// <param name="stepIsCorrect">Indicates whether the step was passed correctly or not.</param>
        /// <param name="stepDescription">A description of the step that was passed for the Log.</param>
        private void OnActionPassedHandler(int stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepId == id && stepDescription == description)
            {
                if (isActive == false)
                {
                    Debug.LogWarning($"Step {id} is not active. Skipping action.");
                    OnActionAlreadyPassedHandler(stepId, stepIsCorrect, stepDescription);
                    return;
                }
                Debug.Log($"[SUCCESS] ID: {stepId}, Desc: {stepDescription}");
                GameManager.EventManager.Instance.ActionCorrectlyPassed(stepId, stepIsCorrect, stepDescription);
                isActive = false; // Deactivate the step after it has been passed
            }
        }

        /// <summary>
        /// Handles the case when an action is not passed correctly.
        /// </summary>
        /// <remarks>
        /// This method is called when an action is not passed correctly. It checks if the step is active and if the ID and description match. to execute the step selected.
        /// </remarks>
        /// <param name="stepId">The ID of the step that was not passed.</param>
        /// <param name="stepIsCorrect">Indicates whether the step was passed correctly or not.</param>
        /// <param name="stepDescription">A description of the step that was not passed for the Log.</param>
        private void OnActionNotPassedHandler(int stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepId == id && stepDescription == description)
            {
                if (isActive == false)
                {
                    Debug.LogWarning($"Step {id} is not active. Skipping action.");
                    OnActionAlreadyPassedHandler(stepId, stepIsCorrect, stepDescription);
                    return;
                }
                Debug.Log($"[FAILURE] ID: {stepId}, Desc: {stepDescription}");
                GameManager.EventManager.Instance.ActionFailed(stepId, stepIsCorrect, stepDescription);
            }
        }

        /// <summary>
        /// Handles the case when an action has already been passed.
        /// </summary>
        /// <remarks>
        /// This method is called when an action has already been passed, either correctly or incorrectly. It checks if the step is active and if the ID and description match.
        /// </remarks>
        private void OnActionAlreadyPassedHandler(int stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepId == id && stepDescription == description)
            {
                Debug.Log($"[ALREADY] ID: {stepId}, Desc: {stepDescription}");
                GameManager.EventManager.Instance.ActionAlreadyPassed(stepId, stepIsCorrect, stepDescription);
            }
        }

    }
}