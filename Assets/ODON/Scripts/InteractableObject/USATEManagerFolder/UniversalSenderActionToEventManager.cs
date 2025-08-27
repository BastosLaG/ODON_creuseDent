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


    }
}