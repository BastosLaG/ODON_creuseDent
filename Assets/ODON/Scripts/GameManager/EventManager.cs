using System;
using System.Collections.Generic;
using UnityEngine;


namespace ODON.GameManager
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private List<Data.SO_Scenario> scenario;
        [SerializeField] private int eventManagerId = 0;
        public Data.SO_Scenario Scenario => scenario[eventManagerId];
        public static EventManager Instance { get; private set; }

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

        }

        void Start()
        {
            Scenario.SetScenario(Scenario.Values);

            Scenario.OnActionPassed += ActionCorrectlyPassed;
            Scenario.OnActionFailed += ActionFailed;
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            Scenario.OnActionPassed -= ActionCorrectlyPassed;
            Scenario.OnActionFailed -= ActionFailed;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        private void ActionCorrectlyPassed(Data.E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            Debug.Log($"Action {stepId} passed: {stepIsCorrect}. Description: {stepDescription}");
            // TODO : Handle the action success logic here, e.g., update the scenario or trigger the next step.
        }

        private void ActionFailed(Data.E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            Debug.LogError($"Action {stepId} failed: {stepIsCorrect}. Description: {stepDescription}");
            // TODO : Handle the action failure logic here, e.g., show a message to the player or log the error.
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////

    }
}