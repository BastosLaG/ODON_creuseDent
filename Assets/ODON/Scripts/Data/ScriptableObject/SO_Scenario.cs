using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.Data
{
    /// <summary>
    /// ScriptableObject representing a scenario in the ODON system.
    /// Manages a sequence of steps, tracks progress, and handles events for step validation and scenario progression.
    /// </summary>
    [CreateAssetMenu(fileName = "NewScenario", menuName = "ODON/Scenario", order = 1)]
    public class SO_Scenario : ScriptableObject
    {
        /// <summary>
        /// Event invoked when an action is successfully passed.
        /// </summary>
        public event Action<E_NameActionInteractable, bool, string> OnActionPassed;

        /// <summary>
        /// Event invoked when a new action is set.
        /// </summary>
        public event Action<SO_Step> OnSetNewAction;

        /// <summary>
        /// Event invoked when an action fails.
        /// </summary>
        public event Action<E_NameActionInteractable, bool, string> OnActionFailed;

        /// <summary>
        /// List of steps for the scenario.
        /// </summary>
        [SerializeField] private SO_ListStep key;

        /// <summary>
        /// Ordered list of action IDs for the scenario.
        /// </summary>
        [SerializeField] private List<E_NameActionInteractable> values = new();

        /// <summary>
        /// Index of the current step in the scenario.
        /// </summary>
        [SerializeField] private int currentValueIndex = 0;

        /// <summary>
        /// Gets the list of steps for the scenario.
        /// </summary>
        public SO_ListStep Key => key;

        /// <summary>
        /// Gets the ordered list of action IDs for the scenario.
        /// </summary>
        public List<E_NameActionInteractable> Values => values;

        /// <summary>
        /// Gets the index of the current step in the scenario.
        /// </summary>
        public int CurrentValueIndex => currentValueIndex;

        /// <summary>
        /// Resets the scenario to the first step.
        /// </summary>
        public void ResetScenario()
        {
            currentValueIndex = 0;
        }

        /// <summary>
        /// Sets the scenario steps and reorders the step list to match the provided action IDs.
        /// </summary>
        /// <param name="setValues">List of action IDs to set for the scenario.</param>
        public void SetScenario(List<E_NameActionInteractable> setValues)
        {
            values = new(setValues);

            // Réorganiser les clés dans le même ordre que les valeurs
            List<SO_Step> sortedKey = new();

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

        /// <summary>
        /// Updates the current step index based on the result of the step validation.
        /// Invokes events for action passed, failed, and new action set.
        /// </summary>
        /// <param name="stepId">The action ID of the step.</param>
        /// <param name="stepIsCorrect">Indicates if the step was completed correctly.</param>
        /// <param name="stepDescription">Description of the step.</param>
        public void UpdateCurrentValueIndex(E_NameActionInteractable stepId, bool stepIsCorrect, string stepDescription)
        {
            if (stepIsCorrect)
            {
                if (stepId == values[currentValueIndex])
                {
                    if (currentValueIndex < values.Count - 1)
                    {
                        // Handle action passed
                        Debug.Log($"Step Id : {stepId} are successfuly. with this settings : {stepIsCorrect} and for description {stepDescription}");
                        currentValueIndex++;
                        OnActionPassed?.Invoke(stepId, stepIsCorrect, stepDescription);
                        OnSetNewAction?.Invoke(key.List[currentValueIndex]);
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
            Debug.Log($"Step Id : {stepId} is failed. with this settings : {stepIsCorrect} and for description {stepDescription}");
            OnActionFailed?.Invoke(stepId, stepIsCorrect, stepDescription);
        }

        /// <summary>
        /// Invokes the event to set a new action for the current step.
        /// </summary>
        public void InvokeOnSetNewAction()
        {
            OnSetNewAction?.Invoke(key.List[currentValueIndex]);
        }
    }
}