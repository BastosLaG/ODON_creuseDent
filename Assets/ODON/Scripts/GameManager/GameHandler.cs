using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace ODON.GameManager
{
    public class GameHandler : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Singleton instance of GameHandler.
        /// </summary>
        /// <remarks>
        /// This class manages the game state, including patient data, interactive items, and security items.
        /// It ensures that only one instance of GameHandler exists throughout the game.
        private static GameHandler instance;
        public static GameHandler Instance => instance;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Erreur Data
        [Header("Error Data")]
        [SerializeField] private List<string> errorData = new();
        public List<string> ErrorData => errorData;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Patient Data
        /// <summary>
        /// The ID of the current technique being used.
        /// </summary>
        /// <remarks>
        /// This ID corresponds to the technique currently being performed by the player.
        /// </remarks>
        [Header("Patient Data")]
        [SerializeField] private Data.PatientData patientData;
        public UnityEvent<Data.PatientData> onUpdatePatientData = new();
        [SerializeField] private Data.PatientState patientState = Data.PatientState.InWaitingRoom;
        public Data.PatientData PatientData
        {
            get => patientData;
            set => patientData = value;
        }
        public Data.PatientState PatientState
        {
            get => patientState;
            set => patientState = value;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////
        #region 
        [Header("Player Data")]
        [SerializeField] private InputActionManager playerInput;
        public InputActionManager PlayerInput
        {
            get => playerInput;
            private set => playerInput = value;
        }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods
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

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Patient Management
        public void PatientInCabinet()
        {
            patientState = Data.PatientState.InCabinet;
        }

        public void PatientInBed()
        {
            patientState = Data.PatientState.InBed;
        }

        public void PatientInWaitingRoom()
        {
            patientState = Data.PatientState.InWaitingRoom;
        }

        public void PatientLeaving()
        {
            patientState = Data.PatientState.Leaving;
        }

        public void UpdatePatientData(Data.PatientMetaData patientMetaData)
        {
            PatientData.PatientMetaData = patientMetaData;
            onUpdatePatientData?.Invoke(PatientData);
        }
        #endregion
    }
}