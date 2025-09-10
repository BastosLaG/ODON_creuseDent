using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;


namespace ODON.GameManager
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private List<Data.SO_Scenario> scenario;
        [SerializeField] private int eventManagerId = 0;
        public Data.SO_Scenario Scenario => (scenario != null && eventManagerId >= 0 && eventManagerId < scenario.Count)
                                            ? scenario[eventManagerId]
                                            : null;

        public static EventManager Instance { get; private set; }
        public Data.SO_Step CurrentStep { get; internal set; }

        private InputActionManager playerInput;
        private InputAction[] triggerActions = new InputAction[2];
        
        ///////////////////////////////////////////////////////////////////////////////////

        #region Init
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

        private void OnEnable()
        {
            Scenario.OnActionPassed += ActionCorrectlyPassed;
            Scenario.OnActionFailed += ActionFailed;

            StartCoroutine(InvokeOnSetNewActionAfterFrame());

            // In case triggerAction was found earlier and OnEnable is called again
            foreach (InputAction triggerAction in triggerActions)
            {
                if (triggerAction != null)
                {
                    triggerAction.started += UIManager.Instance.OnTriggerStarted;
                    triggerAction.Enable();
                }
            }
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            Scenario.OnActionPassed -= ActionCorrectlyPassed;
            Scenario.OnActionFailed -= ActionFailed;

            foreach (InputAction triggerAction in triggerActions)
            {
                if (triggerAction != null)
                {
                    triggerAction.started -= UIManager.Instance.OnTriggerStarted;
                    triggerAction.Enable();
                }
            }
        }

        void Start()
        {
            Scenario.SetScenario(Scenario.Values);
            CurrentStep = Scenario.Key.List[0];
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////

        #region Private Methods
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
        /// <param name="step"></param>
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

        private IEnumerator InvokeOnSetNewActionAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            Scenario.InvokeOnSetNewAction();
        }

    }
}