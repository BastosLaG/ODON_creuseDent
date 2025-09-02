using System.Collections;
using UnityEngine;
[RequireComponent(typeof(PatientPathFollower))]
public class PatientSwap : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds7 = new(7);
    private static readonly WaitForSeconds _waitForSeconds10 = new(10);
    private static readonly WaitForSeconds _waitForSeconds15 = new(15);
    private static readonly WaitForSeconds _waitForSeconds5 = new(5);

    public enum PatientNames
    {
        George,
        Jaqueline,
        Rose,
        Samanta,
        Stephan,
        Thomas,
        Wiliam,
        Yvette
    }

    [System.Serializable]
    private class PatientMouth
    {
        public PatientNames patientName;
        public GameObject patientBody;
        public PatientMouthPos patientMouthPos;
    }
    private PatientPathFollower patientPathFollower;
    [SerializeField] private PatientNames patientToSwap = PatientNames.George;
    private PatientNames actualPatient;
    [SerializeField] private BlendShapesDriver jawDriver;
    [SerializeField] private Transform digue;
    [SerializeField] private PatientMouth[] patientMouths;

    private PatientMouth currentPatient;
    private float currentWeight = 0f;
    private float tempWeight = 0f;

    void Start()
    {
        patientPathFollower = GetComponent<PatientPathFollower>();
        SetPatientMouth(PatientNames.George);
        BentFingers();

        // TESTS, TO REPLACE BY OWN CALLS
        StartCoroutine(PathTest());
    }

    private IEnumerator PathTest()
    {
        yield return _waitForSeconds5;
        patientPathFollower.GoToPoint(0); // Go to the waiting chair
        yield return _waitForSeconds5;
        patientPathFollower.GoToPoint(1); // Go to the chair to give documents
        yield return _waitForSeconds15;
        patientPathFollower.GoToPoint(2); // Go to the bed to get digued
        yield return _waitForSeconds5;
        ChangeMouseState();
        yield return _waitForSeconds10;
        ChangeMouseState();
        yield return _waitForSeconds5;
        patientPathFollower.GoToPoint(1); // Go back to the chair to pay the bill
        yield return _waitForSeconds7;
        patientPathFollower.GoToPoint(3); // Leave the cabinet
        yield return _waitForSeconds10;
        if ((int)currentPatient.patientName + 1 < System.Enum.GetNames(typeof(PatientNames)).Length)
        {
            PatientNames nextPatient = (PatientNames)((int)currentPatient.patientName + 1);
            SetPatientMouth(nextPatient);
            StartCoroutine(PathTest());
        }
    }

    private void BentFingers()
    {
        foreach (PatientMouth mouth in patientMouths)
        {
            mouth.patientBody.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(50, 100);
        }
    }

    void Update()
    {
        if (patientToSwap != actualPatient)
        {
            SetPatientMouth(patientToSwap);
        }
    }

    public void SetPatientMouth(PatientNames patientNames)
    {
        actualPatient = patientToSwap;
        foreach (PatientMouth patientMouth in patientMouths)
        {
            if (patientMouth.patientName == patientNames)
            {
                jawDriver.transform.localPosition = patientMouth.patientMouthPos.JawPos;
                digue.localPosition = patientMouth.patientMouthPos.diguePos;
                patientMouth.patientBody.SetActive(true);
                currentPatient = patientMouth;
            }
            else
            {
                patientMouth.patientBody.SetActive(false);
                patientMouth.patientBody.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = false;
            }
        }
    }

    public void ChangeMouseState()
    {
        tempWeight = currentWeight == 100 ? 0 : 100;
        currentWeight = tempWeight;
        jawDriver.GoToValue("mouth_open", currentWeight);
    }
}
