using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "ODON/Scenario", order = 1)]
    public class SO_Scenario : ScriptableObject
    {
        [SerializeField] private SO_ListStep key;
        [SerializeField] private List<E_NameActionInteractable> values = new ();
        [SerializeField] private int currentValueIndex = 0;
        public SO_ListStep Key => key;
        public List<E_NameActionInteractable> Values => values;
        public int CurrentValueIndex => currentValueIndex;

        public void ResetScenario()
        {
            currentValueIndex = 0;
        }

        public void SetScenario(List<E_NameActionInteractable> setValues)
        {
            values = new List<E_NameActionInteractable>(setValues);

            // Réorganiser les clés dans le même ordre que les valeurs
            List<SO_Step> sortedKey = new List<SO_Step>();

            foreach (var value in values)
            {
                SO_Step matchingStep = key.List.Find(step => step.Id == value);
                if (matchingStep != null)
                {
                    sortedKey.Add(matchingStep);
                }
                else
                {
                    Debug.LogError($"No matching SO_Step found for action '{value}'.");
                }
            }

            key.List = sortedKey;

            ResetScenario();
        }

        public event Action<E_NameActionInteractable, bool, string> OnActionPassed;
        public event Action<E_NameActionInteractable, bool, string> OnActionFailed;

        public void UpdateCurrentValueIndex(E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepIsCorrect)
            {
                if (stepId == values[currentValueIndex])
                {
                    if (currentValueIndex < values.Count - 1)
                    {
                        // Handle action passed
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

            //Handle action not passed
            OnActionFailed?.Invoke(stepId, stepIsCorrect, stepDescription);
        }
    }
}