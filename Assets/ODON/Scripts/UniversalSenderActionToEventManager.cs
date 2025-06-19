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

        public bool CheckIfValidatorObjectsAreValid()
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
    }
}