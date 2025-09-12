using System;
using System.Collections;
using UnityEngine;

namespace ODON
{
    /// <summary>
    /// Manages patient swapping and mouth state transitions in the ODON application.
    /// Handles movement between points, blend shape control for mouth opening, and switching between patient models.
    /// </summary>
    [RequireComponent(typeof(PatientPathFollower))]
    public class PatientSwap : MonoBehaviour
    {
        /// <summary>
        /// Wait time used when patient is in bed before changing mouth state.
        /// </summary>
        private static readonly WaitForSeconds _waitForSeconds10 = new(10);

        /// <summary>
        /// Reference to the PatientPathFollower component for movement control.
        /// </summary>
        private PatientPathFollower patientPathFollower;

        /// <summary>
        /// Reference to the BlendShapesDriver for controlling jaw blend shapes.
        /// </summary>
        [SerializeField] private BlendShapesDriver jawDriver;

        /// <summary>
        /// Transform representing the digue (dam) position.
        /// </summary>
        [SerializeField] private Transform digue;

        /// <summary>
        /// Array of patient metadata containing body and mouth data.
        /// </summary>
        [SerializeField] private Data.PatientMeta[] patientsMeta;

        /// <summary>
        /// Desired patient state.
        /// </summary>
        [SerializeField] private Data.PatientState patientState;

        /// <summary>
        /// Current patient state.
        /// </summary>
        [SerializeField] private Data.PatientState currentPatientState;

        /// <summary>
        /// Current blend shape weight for mouth opening.
        /// </summary>
        private float currentWeight = 0f;

        /// <summary>
        /// Temporary blend shape weight for toggling mouth state.
        /// </summary>
        private float tempWeight = 0f;

        /// <summary>
        /// Initializes the PatientSwap component and sets up the initial patient.
        /// </summary>
        void Start()
        {
            patientPathFollower = GetComponent<PatientPathFollower>();

            InitNewPatient();
        }

        /// <summary>
        /// Updates the patient state and triggers movement or mouth state changes as needed.
        /// </summary>
        private void Update()
        {
            patientState = GameManager.GameHandler.Instance.PatientState;
            if (patientState != currentPatientState)
            {
                currentPatientState = patientState;
                if (currentPatientState == Data.PatientState.InWaitingRoom)
                {
                    patientPathFollower.GoToPoint(0);
                }
                else if (currentPatientState == Data.PatientState.InCabinet)
                {
                    patientPathFollower.GoToPoint(1);
                }
                else if (currentPatientState == Data.PatientState.InBed)
                {
                    StartCoroutine(HandleInBedState());
                }
                else if (currentPatientState == Data.PatientState.Leaving)
                {
                    ChangeMouseState();
                    patientPathFollower.GoToPoint(3);
                }
            }
        }

        /// <summary>
        /// Coroutine to handle patient actions when in bed, including waiting and changing mouth state.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator HandleInBedState()
        {
            patientPathFollower.GoToPoint(2);
            yield return _waitForSeconds10;
            ChangeMouseState();
        }

        /// <summary>
        /// Bends the fingers of all patient models by setting the blend shape weight.
        /// </summary>
        private void BentFingers()
        {
            foreach (Data.PatientMeta patientMeta in patientsMeta)
            {
                patientMeta.patientBody.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(50, 100);
            }
        }

        /// <summary>
        /// Selects a random patient name from the PatientNames enum.
        /// </summary>
        /// <returns>A randomly selected PatientNames value.</returns>
        private Data.PatientNames RandomPatient()
        {
            return (Data.PatientNames)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Data.PatientNames)).Length);
        }

        /// <summary>
        /// Sets the active patient mouth and updates positions and patient data.
        /// </summary>
        /// <param name="patientNames">The patient name to activate.</param>
        public void SetPatientMouth(Data.PatientNames patientNames)
        {
            Data.PatientMeta patientMeta = Array.Find(patientsMeta, meta => meta.patientMetaData.PatientName == patientNames);

            foreach (Data.PatientMeta item in patientsMeta)
            {
                if (item.patientBody != patientMeta.patientBody)
                {
                    item.patientBody.SetActive(false);
                    item.patientBody.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
                }
            }
            jawDriver.transform.localPosition = patientMeta.patientMetaData.JawPos;
            digue.localPosition = patientMeta.patientMetaData.DiguePos;
            patientMeta.patientBody.SetActive(true);

            GameManager.GameHandler.Instance.UpdatePatientData(patientMeta.patientMetaData);
            
        }

        /// <summary>
        /// Toggles the mouth state by changing the blend shape weight for mouth opening.
        /// </summary>
        public void ChangeMouseState()
        {
            tempWeight = currentWeight == 100 ? 0 : 100;
            currentWeight = tempWeight;
            jawDriver.GoToValue("mouth_open", currentWeight);
        }

        /// <summary>
        /// Initializes a new patient by selecting a random patient, setting mouth, bending fingers, and moving to the first point.
        /// </summary>
        public void InitNewPatient()
        {
            Data.PatientNames randomPatientName = RandomPatient();
            SetPatientMouth(randomPatientName);
            BentFingers();
            patientPathFollower.GoToPoint(0);
        }
    }
}