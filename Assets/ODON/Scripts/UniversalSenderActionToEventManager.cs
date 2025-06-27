using UnityEngine;


namespace ODON
{
    public class UniversalSenderActionToEventManager : MonoBehaviour
    {
        [SerializeField] private Data.SO_Step step;
        public Data.SO_Step Step => step;

        [SerializeField] private Data.Struct_VRValidatorObject[] validatorObjects;
        public Data.Struct_VRValidatorObject[] ValidatorObjects => validatorObjects;

        private void Start()
        {
            GameManager.HighlightsManager.Instance.RegisterStep(step, this);
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

        public void TryValidateCurrentItem()
        {
            if (enabled == false) return;
            if (!CheckIfValidatorObjectsAreValid()) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step);
        }

        public void TryValidateCurrentItem(bool stepIsCorrect)
        {
            if (enabled == false) return;
            if (!CheckIfValidatorObjectsAreValid()) return;
            GameManager.EventManager.Instance.TryValidateCurrentItem(Step, stepIsCorrect);
        }
    }
}