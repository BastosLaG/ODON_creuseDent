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

        #region Initialization
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
            Scenario.ResetScenario();

            Scenario.OnActionPassed += ActionCorrectlyPassed;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////
        

        public void ActionCorrectlyPassed(Data.E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            Scenario.UpdateCurrentValueIndex(stepId, stepIsCorrect, stepDescription);
        }
    }
}