using UnityEngine;


namespace ODON
{
    public class UniversalSenderActionToEventManager : MonoBehaviour
    {
        [SerializeField] private Data.SO_Step step;
        public Data.SO_Step Step => step;

        [SerializeField] private Data.Struct_VRValidatorObject[] validatorObjects;

        private void Start()
        {
            GameManager.HighlightsManager.Instance.RegisterStep(step, this);
        }

        public void SendActiveCheckpointProgress()
        {
            if (step == null)
            {
                Debug.LogError("Step is not assigned in UniversalSenderActionToEventManager.");
                return;
            }
            GameManager.EventManager.Instance.TryValidateCurrentItem(step);
        }

        /// <summary>
        /// Checks if the current step is active by verifying whether the <see cref="Outline"/> component
        /// on the GameObject is enabled.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <see cref="Outline"/> component is enabled; otherwise, <c>false</c>.
        /// Logs a warning if the <see cref="Outline"/> component is not enabled.
        /// </returns>
        public bool CheckIfStepIsActive()
        {
            if (GetComponent<Outline>()?.enabled == true)
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"Outline is not enabled on {this.transform.name}. Step is not active.");
                return false;
            }
        }

        private bool CheckIfValidatorObjectsAreValid()
        {
            if (validatorObjects == null || validatorObjects.Length == 0) return true;
            foreach (Data.Struct_VRValidatorObject validatorObject in validatorObjects)
            {
                if (!validatorObject.IsValid)
                {
                    return false;
                }
            }
            return true;
        }

        public void TryValdidateCurrentItem()
        {
            if (!CheckIfValidatorObjectsAreValid()) return;
            if (CheckIfStepIsActive())
            {
                GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
            }
            else
            {
                Step.ActionFailed();
            }
        }

        public void TryValdidateCurrentItem(bool stepIsCorrect)
        {
            if (!CheckIfValidatorObjectsAreValid()) return;
            if (CheckIfStepIsActive())
            {
                GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
            }
            else
            {
                Step.ActionFailed();
            }
        }
    }
}