using UnityEngine;
using System.Collections.Generic;
using ODON.Data;

namespace ODON.GameManager
{
    public class HighlightsManager : MonoBehaviour
    {
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

        private readonly Dictionary<SO_Step, MonoBehaviour> stepMap = new();

        /// <summary>
        /// Registers a step with its corresponding behaviour.
        /// This method ensures that the step is only registered once and initializes highlighting if necessary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="step"></param>
        /// <param name="behaviour"></param>
        public void RegisterStep<T>(SO_Step step, T behaviour) where T : MonoBehaviour
        {
            if (!stepMap.ContainsKey(step))
            {
                stepMap.Add(step, behaviour);

                if (!behaviour.gameObject.TryGetComponent<Outline>(out _))
                {
                    InitHighLight(behaviour.gameObject);
                }
            }
        }

        public T GetStepBehaviour<T>(SO_Step step) where T : MonoBehaviour
        {
            if (stepMap.TryGetValue(step, out MonoBehaviour mb))
            {
                return mb as T;
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
            Manager_outline.AddOutline(obj);
        }
    }
}
