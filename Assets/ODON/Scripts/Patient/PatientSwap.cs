using System;
using System.Collections;
using UnityEngine;

namespace ODON
{
    [RequireComponent(typeof(PatientPathFollower))]
    public class PatientSwap : MonoBehaviour
    {
        private static readonly WaitForSeconds _waitForSeconds10 = new(10);

        private PatientPathFollower patientPathFollower;

        [SerializeField] private BlendShapesDriver jawDriver;
        [SerializeField] private Transform digue;
        [SerializeField] private Data.PatientMeta[] patientsMeta;

        [SerializeField] private Data.PatientState patientState;
        [SerializeField] private Data.PatientState currentPatientState;

        private float currentWeight = 0f;
        private float tempWeight = 0f;

        void Start()
        {
            patientPathFollower = GetComponent<PatientPathFollower>();

            InitNewPatient();
        }

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

        private IEnumerator HandleInBedState()
        {
            patientPathFollower.GoToPoint(2);
            yield return _waitForSeconds10;
            ChangeMouseState();
        }

        private void BentFingers()
        {
            foreach (Data.PatientMeta patientMeta in patientsMeta)
            {
                patientMeta.patientBody.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(50, 100);
            }
        }

        private Data.PatientNames RandomPatient()
        {
            return (Data.PatientNames)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Data.PatientNames)).Length);
        }

        public void SetPatientMouth(Data.PatientNames patientNames)
        {
            Data.PatientMeta patientMeta = Array.Find(patientsMeta, meta => meta.patientMetaData.PatientName == patientNames);

            Debug.Log($"Setting mouth for patient: {patientNames} | PatientMeta: {patientMeta.patientMetaData.PatientName.ToString()}");

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

            GameManager.GameHandler.Instance.PatientData.LoadMetaData(patientMeta.patientMetaData.PatientName.ToString(), patientMeta.patientMetaData.Age, patientMeta.patientMetaData.Gender);
            GameManager.UIManager.Instance.UpdatePatientData();
        }

        public void ChangeMouseState()
        {
            tempWeight = currentWeight == 100 ? 0 : 100;
            currentWeight = tempWeight;
            jawDriver.GoToValue("mouth_open", currentWeight);
        }

        public void InitNewPatient()
        {
            Data.PatientNames randomPatientName = RandomPatient();
            SetPatientMouth(randomPatientName);
            BentFingers();
            patientPathFollower.GoToPoint(0);
        }
    }
}