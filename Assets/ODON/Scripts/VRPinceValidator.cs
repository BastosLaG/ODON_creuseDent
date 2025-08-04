using UnityEngine;
using UnityEngine.Events;

namespace ODON
{
    public class VRPinceValidator : MonoBehaviour
    {
        #region Properties
        public UnityEvent OnActionValidated;
        [SerializeField] private UniversalSenderActionToEventManager uSATEManager;
        #endregion

        /////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods
        private void Awake()
        {
            if (uSATEManager == null)
            {
                Debug.LogError("UniversalSenderActionToEventManager is not assigned in VRPinceValidator.");
            }
        }

        private void Start()
        {
            OnActionValidated.AddListener(uSATEManager.TryValidateCurrentItem);
        }

        private void OnDisable()
        {
            OnActionValidated.RemoveListener(uSATEManager.TryValidateCurrentItem);
        }
        #endregion

        /////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void ValidateAction()
        {
            OnActionValidated.Invoke();
        }
        #endregion
    }
}
