using UnityEngine;


namespace ODON.UsateManager
{
    public abstract class UniversalSenderActionToEventManager : MonoBehaviour
    {
        [Header("Default Settings")]
        [SerializeField] protected Data.SO_Step step;
        public Data.SO_Step Step => step;

        [SerializeField] protected Transform TargetHighlight = null;

        protected void Start()
        {
            if (TargetHighlight == null)
            {
                TargetHighlight = transform;
            }
            GameManager.HighlightsManager.Instance.RegisterStep(step, TargetHighlight);
        }

        public bool IsValidStep()
        {
            Debug.Log($"IsValidStep called on {gameObject.name} for step {Step.name} and we need {GameManager.EventManager.Instance.CurrentStep} \n| Result: {Step == GameManager.EventManager.Instance.CurrentStep}");
            return Step == GameManager.EventManager.Instance.CurrentStep;
        }

        public virtual void TryValidateCurrentItem()
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
        }

        public virtual void TryValidateCurrentItem(bool stepIsCorrect)
        {
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
        }
    }
}