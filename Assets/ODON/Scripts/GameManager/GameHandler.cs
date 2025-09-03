using System.Collections.Generic;
using UnityEngine;

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
        #endregion
    }
}