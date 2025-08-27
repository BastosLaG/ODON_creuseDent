using UnityEngine;


namespace ODON.UsateManager
{
    public abstract class UniversalSenderActionToEventManager : MonoBehaviour
    {
        [SerializeField] protected Data.SO_Step step;
        public Data.SO_Step Step => step;

        protected void Start()
        {
            GameManager.HighlightsManager.Instance.RegisterStep(step, this);
        }
    }
}