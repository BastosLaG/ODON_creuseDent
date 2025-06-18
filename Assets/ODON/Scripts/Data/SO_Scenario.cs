using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "ODON/Scenario", order = 1)]
    public class SO_Scenario : ScriptableObject
    {
        [SerializeField] private List<SO_Step> key = new List<SO_Step>();
        [SerializeField] private List<E_NameActionInteractable> values = new List<E_NameActionInteractable>();
        [SerializeField] private int currentValueIndex = 0;

        void OnEnable()
        {
            // Debug.Log($"Scenario {name} enabled with {key.Count} steps.");

            for (int i = 0; i < values.Count; i++)
            {
                // Debug.Log($"Value {i}: {values[i]}");
            }
        }

        public void ResetScenario()
        {
            currentValueIndex = 0;
            // Debug.Log($"Scenario {name} reset. Current value index set to {currentValueIndex}.");
        }


        public event Action<E_NameActionInteractable, bool, string> OnActionPassed;
        public void UpdateCurrentValueIndex(E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepIsCorrect)
            {
                if (stepId == values[currentValueIndex])
                {
                    if (currentValueIndex < values.Count - 1)
                    {
                        currentValueIndex++;
                        OnActionPassed?.Invoke(stepId, stepIsCorrect, stepDescription);
                        return;
                    }
                    else
                    {
                        // TODO handle End of Scenario
                        Debug.LogWarning("Current value index is already at the last value.");
                        return;
                    }
                }
            }
            
            //TODO handle action not passed
            Debug.Log($"Error: Action {stepId} not passed because it's not the right time . {values[currentValueIndex]} expected at index {currentValueIndex}."); 
        }
     
        private void NextStep()
        {
        }
    }
}