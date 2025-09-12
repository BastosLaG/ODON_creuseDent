using UnityEngine;
using System.Collections.Generic;

namespace ODON.GameManager
{
    /// <summary>
    /// Manages the highlighting of steps in the ODON application.
    /// Registers steps with their highlight targets, controls outline visibility, and provides access to step targets.
    /// </summary>
    public class HighlightsManager : MonoBehaviour
    {
        /// <summary>
        /// Dictionary mapping steps to their corresponding highlight targets.
        /// </summary>
        private readonly Dictionary<Data.SO_Step, Transform> stepMap = new();

        /// <summary>
        /// Singleton instance of HighlightsManager.
        /// </summary>
        private static HighlightsManager instance;

        /// <summary>
        /// Gets the singleton instance of HighlightsManager.
        /// </summary>
        public static HighlightsManager Instance => instance;

        /// <summary>
        /// Initializes the singleton instance on Awake.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Registers the OnSetNewAction event listener on Start.
        /// </summary>
        void Start()
        {
            EventManager.Instance.Scenario.OnSetNewAction += OnSetNewAction;
        }

        /// <summary>
        /// Unregisters the OnSetNewAction event listener on disable.
        /// </summary>
        void OnDisable()
        {
            EventManager.Instance.Scenario.OnSetNewAction -= OnSetNewAction;
        }

        /// <summary>
        /// Registers a step with its corresponding highlights target.
        /// This method ensures that the step is only registered once and initializes highlighting if necessary.
        /// </summary>
        /// <param name="step">The scenario step to register.</param>
        /// <param name="target">The transform to highlight for this step.</param>
        public void RegisterStep(Data.SO_Step step, Transform target)
        {
            if (!stepMap.ContainsKey(step))
            {
                stepMap.Add(step, target);

                // Debug.Log($"Registered step {step.Id} with behaviour {target.GetType().Name}");
                if (!target.TryGetComponent<Outline>(out _))
                {
                    GameObject obj = target.gameObject;
                    InitHighLight(obj);

                    // Debug.Log($"Added outline to {obj.name} for step {step.Id}");
                }
            }
        }

        /// <summary>
        /// Gets the highlight target transform for a given step.
        /// </summary>
        /// <param name="step">The scenario step.</param>
        /// <returns>The transform associated with the step, or null if not found.</returns>
        public Transform GetStepBehaviour(Data.SO_Step step)
        {
            if (stepMap.TryGetValue(step, out Transform target))
            {
                return target;
            }
            return null;
        }

        /// <summary>
        /// Initializes the highlight for a given GameObject.
        /// This method adds an outline component to the GameObject if it does not already have one.
        /// </summary>
        /// <param name="obj">The GameObject to initialize highlighting for.</param>
        public void InitHighLight(GameObject obj)
        {
            Outline outline = obj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
        }

        /// <summary>
        /// Enables the outline for the target associated with the given step when a new action is set.
        /// </summary>
        /// <param name="step">The scenario step for which to enable highlighting.</param>
        public void OnSetNewAction(Data.SO_Step step)
        {
            if (stepMap.TryGetValue(step, out Transform target))
            {
                if (target.gameObject.TryGetComponent<Outline>(out var outline))
                {
                    outline.enabled = true;
                    // Debug.Log($"Enabled outline for step {step.Id}");
                }
                else
                {
                    Debug.LogWarning($"No Outline component found on {target.name} for step {step.Id}");
                }
            }
            else
            {
                Debug.LogError($"Step {step.Id} not registered in HighlightsManager.");
            }
        }
    }
}
