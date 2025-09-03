using System.Collections;
using UnityEngine;

namespace ODON
{
    [RequireComponent(typeof(PatientPathFollower))]
    public class PatientSwap : MonoBehaviour
    {
        private static readonly WaitForSeconds _waitForSeconds10 = new(10);

        private PatientPathFollower patientPathFollower;
        [SerializeField] private Data.PatientNames patientToSwap;

        [SerializeField] private BlendShapesDriver jawDriver;
        [SerializeField] private Transform digue;
        [SerializeField] private Data.PatientMouth[] patientMouths;

        [SerializeField] private Data.PatientState patientState;
        [SerializeField] private Data.PatientState currentPatientState;

        private Data.PatientMouth currentPatient;
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
            foreach (Data.PatientMouth mouth in patientMouths)
            {
                mouth.patientBody.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(50, 100);
            }
        }

        private Data.PatientNames RandomPatient()
        {
            return (Data.PatientNames)Random.Range(0, System.Enum.GetNames(typeof(Data.PatientNames)).Length - 1);
        }

        public void SetPatientMouth(Data.PatientNames patientNames)
        {
            foreach (Data.PatientMouth patientMouth in patientMouths)
            {
                if (patientMouth.patientName == patientNames)
                {
                    jawDriver.transform.localPosition = patientMouth.patientMouthPos.JawPos;
                    digue.localPosition = patientMouth.patientMouthPos.DiguePos;
                    patientMouth.patientBody.SetActive(true);
                    currentPatient = patientMouth;
                }
                else
                {
                    patientMouth.patientBody.SetActive(false);
                    patientMouth.patientBody.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
                }
            }

            GameManager.GameHandler.Instance.PatientData[GameManager.GameHandler.Instance.PatientDataIndex].PatientName = System.Enum.GetName(typeof(Data.PatientNames), patientToSwap);
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
            GameManager.GameHandler.Instance.PatientData[GameManager.GameHandler.Instance.PatientDataIndex].PatientName = System.Enum.GetName(typeof(Data.PatientNames), randomPatientName);
            BentFingers();
            patientPathFollower.GoToPoint(0);
        }
    }
}