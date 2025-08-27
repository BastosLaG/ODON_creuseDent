using UnityEngine;
using System.Collections.Generic;

namespace ODON.GameManager
{
    public class HighlightsManager : MonoBehaviour
    {
        private readonly Dictionary<Data.SO_Step, Transform> stepMap = new();

        private static HighlightsManager instance;
        public static HighlightsManager Instance => instance;

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

        void Start()
        {
            EventManager.Instance.Scenario.OnSetNewAction += OnSetNewAction;
        }

        void OnDisable()
        {
            EventManager.Instance.Scenario.OnSetNewAction -= OnSetNewAction;
        }

        /// <summary>
        /// Registers a step with its corresponding highlights target.
        /// This method ensures that the step is only registered once and initializes highlighting if necessary.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="target"></param>
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
        /// <param name="obj"></param>
        public void InitHighLight(GameObject obj)
        {
            Outline outline = obj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
        }

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
