using UnityEngine;


namespace ODON
{
    public class UniversalSenderActionToEventManager : MonoBehaviour
    {
        [SerializeField] private Data.SO_Step step;
        public Data.SO_Step Step => step;

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

        public bool CheckIfStepIsActive()
        {
            if (GetComponent<Outline>()?.enabled == true)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("Outline is not enabled on the GameObject. Step is not active.");
                return false;
            }
        }
    }
}