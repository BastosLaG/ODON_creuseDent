using System;
using UnityEngine;


namespace ODON
{
    public class Manager_outline : MonoBehaviour
    {

        private static Manager_outline instance = null;
        public static Manager_outline Instance => instance;
        [SerializeField] private Data.PoseSettings poseSettings;
        public Data.PoseSettings PoseSettings => poseSettings;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            poseSettings.Reset();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        #region Enabled/Disabled Outline

        public void EnableOutline()
        {
            poseSettings.SwitchOutlineIncremente();
        }

        public void DisableOutline()
        {
            poseSettings.SwitchOutlineDecremente();
        }
    }
    #endregion
}