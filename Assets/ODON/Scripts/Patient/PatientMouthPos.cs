using UnityEngine;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "PatientMetaData", menuName = "ScriptableObjects/PatientMetaData")]
    public class PatientMetaData : ScriptableObject
    {
        public PatientNames PatientName;
        public int Age;
        public Gender Gender;
        public Vector3 JawPos;
        public Vector3 DiguePos;
    }
}