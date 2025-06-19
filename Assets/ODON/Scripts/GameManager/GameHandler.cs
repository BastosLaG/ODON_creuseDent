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
        
        #region Scenario And Erreur Data
        /// <summary>
        /// The scenario data for the game.
        /// </summary>
        /// <remarks>
        /// This list contains the scenario data that defines the game flow and events.
        /// </remarks>
        [Header("Scenario")]
        [Tooltip("The scenario data for the game.")]
        [SerializeField] private List<Data.SO_Scenario> scenario;

        [SerializeField] private List<string> errorData = new ();
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
        [SerializeField] private Data.PatientData[] patientData;
        [SerializeField] private int patientDataIndex = 0;
        [SerializeField] private Data.PatientState patientState = Data.PatientState.InWaitingRoom;
        [SerializeField] private bool isPatientInRoom = false;
        [SerializeField] private bool isPatientInBed = false;
        public Data.PatientData[] PatientData
        {
            get => patientData;
            private set => patientData = value;
        }
        public int PatientDataIndex
        {
            get => patientDataIndex;
            private set => patientDataIndex = value;
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

        void Start()
        {
            UIManager.Instance.InitClipBoard();
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Patient Management
        public void NewPatientEnterOnRoom()
        {
            if (!isPatientInRoom)
            {
                patientState = Data.PatientState.InCabinet;
                isPatientInRoom = true;
            }
        }

        public void PatientSitOnBed()
        {
            if (!isPatientInBed)
            {
                isPatientInBed = true;
                patientState = Data.PatientState.InBed;
            }
        }

        public void PatientExitFromRoom()
        {
            patientState = Data.PatientState.InWaitingRoom;
        }
        #endregion
    }
}