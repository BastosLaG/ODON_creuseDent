using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    public struct PatientMouth
    {
        public PatientNames patientName;
        public GameObject patientBody;
        public PatientMouthPos patientMouthPos;
    }
}