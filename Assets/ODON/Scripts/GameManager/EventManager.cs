using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace ODON.GameManager
{
    /// <summary>
    /// Manages scenario events and player input in the ODON application.
    /// Handles scenario progression, step validation, and input actions for triggers.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        /// <summary>
        /// List of scenarios managed by the event manager.
        /// </summary>
        [SerializeField] private List<Data.SO_Scenario> scenario;

        /// <summary>
        /// Index of the currently active scenario.
        /// </summary>
        [SerializeField] private int eventManagerId = 0;

        /// <summary>
        /// Gets the currently active scenario.
        /// </summary>
        public Data.SO_Scenario Scenario => (scenario != null && eventManagerId >= 0 && eventManagerId < scenario.Count)
                                            ? scenario[eventManagerId]
                                            : null;

        /// <summary>
        /// Singleton instance of EventManager.
        /// </summary>
        public static EventManager Instance { get; private set; }

        /// <summary>
        /// Gets or sets the current step in the scenario.
        /// </summary>
        public Data.SO_Step CurrentStep { get; internal set; }

        /// <summary>
        /// Reference to the player's input action manager.
        /// </summary>
        private InputActionManager playerInput;

        /// <summary>
        /// Array of trigger input actions.
        /// </summary>
        public InputAction[] triggerActions = new InputAction[2];

        ///////////////////////////////////////////////////////////////////////////////////

        #region Init

        /// <summary>
        /// Initializes the singleton instance and sets up trigger actions.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerInput = GameHandler.Instance.PlayerInput;

            int i = 0;
            var asset = playerInput.actionAssets[1];
            foreach (var map in asset.actionMaps)
            {
                foreach (var action in map.actions)
                {
                    foreach (InputBinding binding in action.bindings)
                    {
                        if (binding.path.Contains("trigger"))
                        {
                            Debug.Log("Path Save");
                            triggerActions[i] = action;
                            i++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers scenario event listeners and invokes the new action event after frame.
        /// </summary>
        private void OnEnable()
        {
            Scenario.OnActionPassed += ActionCorrectlyPassed;
            Scenario.OnActionFailed += ActionFailed;

            StartCoroutine(InvokeOnSetNewActionAfterFrame());
        }

        /// <summary>
        /// Unregisters scenario event listeners and clears the singleton instance.
        /// </summary>
        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            Scenario.OnActionPassed -= ActionCorrectlyPassed;
            Scenario.OnActionFailed -= ActionFailed;
        }

        /// <summary>
        /// Initializes the scenario and sets the current step.
        /// </summary>
        void Start()
        {
            Scenario.SetScenario(Scenario.Values);
            CurrentStep = Scenario.Key.List[0];
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////

        #region Private Methods

        /// <summary>
        /// Handles logic when an action is correctly passed, such as disabling outlines.
        /// </summary>
        /// <param name="stepId">The action ID of the step.</param>
        /// <param name="stepIsCorrect">Indicates if the step was completed correctly.</param>
        /// <param name="stepDescription">Description of the step.</param>
        private void ActionCorrectlyPassed(Data.E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            // TODO : Handle the action success logic here, e.g., update the scenario or trigger the next step.
            int index = Scenario.Values.IndexOf(stepId);
            if (index >= 0 && index < Scenario.Key.List.Count)
            {
                Transform target = HighlightsManager.Instance.GetStepBehaviour(Scenario.Key.List[index]);
                target.gameObject.GetComponent<Outline>().enabled = false;
            }
            else
            {
                Debug.LogWarning($"Step ID {stepId} not found in Scenario.Values.");
            }
        }

        /// <summary>
        /// Handles logic when an action fails, such as logging errors.
        /// </summary>
        /// <param name="stepId">The action ID of the step.</param>
        /// <param name="stepIsCorrect">Indicates if the step was completed correctly.</param>
        /// <param name="stepDescription">Description of the step.</param>
        private void ActionFailed(Data.E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            // TODO : Handle the action failure logic here, e.g., show a message to the player or log the error.
            GameHandler.Instance.ErrorData.Add(stepDescription);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        /// <summary>
        /// Attempts to validate the current item based on the provided step.
        /// This method checks if the step is valid and updates the scenario's current value index accordingly.
        /// </summary>
        /// <param name="step">The step to validate.</param>
        public void TryValidateCurrentItem(Data.SO_Step step)
        {
            if (step == null)
            {
                Debug.LogError("Step is null.");
                return;
            }

            if (step.Id == Scenario.Values[Scenario.CurrentValueIndex])
            {
                step.ActionPassed();
                CurrentStep = Scenario.Key.List[Scenario.CurrentValueIndex];
            }
            else
            {
                step.ActionFailed();
            }
        }

        /// <summary>
        /// Attempts to validate the current item based on the provided step and correctness flag.
        /// </summary>
        /// <param name="step">The step to validate.</param>
        /// <param name="stepIsCorrect">Indicates if the step is correct.</param>
        public void TryValidateCurrentItem(Data.SO_Step step, bool stepIsCorrect)
        {
            if (step == null)
            {
                Debug.LogError("Step is null.");
                return;
            }

            if (step.Id == Scenario.Values[Scenario.CurrentValueIndex] && stepIsCorrect)
            {
                step.ActionPassed();
                CurrentStep = Scenario.Key.List[Scenario.CurrentValueIndex];
            }
            else
            {
                step.ActionFailed();
            }
        }
        #endregion

        /// <summary>
        /// Coroutine to invoke the OnSetNewAction event after the current frame.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator InvokeOnSetNewActionAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            Scenario.InvokeOnSetNewAction();
        }

    }
}