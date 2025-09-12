using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace ODON.GameManager
{
    /// <summary>
    /// Handles the main game state and data management for the ODON application.
    /// Manages patient data, error tracking, player input, and ensures a single instance throughout the game.
    /// </summary>
    public class GameHandler : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Singleton instance of GameHandler.
        /// Ensures only one instance exists throughout the game.
        /// </summary>
        private static GameHandler instance;

        /// <summary>
        /// Gets the singleton instance of GameHandler.
        /// </summary>
        public static GameHandler Instance => instance;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Erreur Data
        /// <summary>
        /// List of error messages encountered during gameplay.
        /// </summary>
        [Header("Error Data")]
        [SerializeField] private List<string> errorData = new();

        /// <summary>
        /// Gets the list of error messages.
        /// </summary>
        public List<string> ErrorData => errorData;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Patient Data
        /// <summary>
        /// The patient data for the current session.
        /// </summary>
        [Header("Patient Data")]
        [SerializeField] private Data.PatientData patientData;

        /// <summary>
        /// Event invoked when patient data is updated.
        /// </summary>
        public UnityEvent<Data.PatientData> onUpdatePatientData = new();

        /// <summary>
        /// The current state of the patient.
        /// </summary>
        [SerializeField] private Data.PatientState patientState = Data.PatientState.InWaitingRoom;

        /// <summary>
        /// Gets or sets the patient data.
        /// </summary>
        public Data.PatientData PatientData
        {
            get => patientData;
            set => patientData = value;
        }

        /// <summary>
        /// Gets or sets the current patient state.
        /// </summary>
        public Data.PatientState PatientState
        {
            get => patientState;
            set => patientState = value;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////
        #region Player Data
        /// <summary>
        /// Reference to the player's input action manager.
        /// </summary>
        [Header("Player Data")]
        [SerializeField] private InputActionManager playerInput;

        /// <summary>
        /// Gets the player's input action manager.
        /// </summary>
        public InputActionManager PlayerInput
        {
            get => playerInput;
            private set => playerInput = value;
        }
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods
        /// <summary>
        /// Initializes the singleton instance on Awake.
        /// </summary>
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
        /// <summary>
        /// Sets the patient state to InCabinet.
        /// </summary>
        public void PatientInCabinet()
        {
            patientState = Data.PatientState.InCabinet;
        }

        /// <summary>
        /// Sets the patient state to InBed.
        /// </summary>
        public void PatientInBed()
        {
            patientState = Data.PatientState.InBed;
        }

        /// <summary>
        /// Sets the patient state to InWaitingRoom.
        /// </summary>
        public void PatientInWaitingRoom()
        {
            patientState = Data.PatientState.InWaitingRoom;
        }

        /// <summary>
        /// Sets the patient state to Leaving.
        /// </summary>
        public void PatientLeaving()
        {
            patientState = Data.PatientState.Leaving;
        }

        /// <summary>
        /// Updates the patient data and invokes the update event.
        /// </summary>
        /// <param name="patientMetaData">The new patient metadata.</param>
        public void UpdatePatientData(Data.SO_PatientMetaData patientMetaData)
        {
            PatientData.PatientMetaData = patientMetaData;
            onUpdatePatientData?.Invoke(PatientData);
        }
        #endregion
    }
}