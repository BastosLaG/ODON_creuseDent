using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// ScriptableObject containing metadata for a patient in the ODON system.
    /// Stores patient name, age, gender, and jaw/digue positions.
    /// </summary>
    [CreateAssetMenu(fileName = "PatientMetaData", menuName = "ScriptableObjects/PatientMetaData")]
    public class SO_PatientMetaData : ScriptableObject
    {
        /// <summary>
        /// The name of the patient.
        /// </summary>
        public PatientNames PatientName;

        /// <summary>
        /// The age of the patient.
        /// </summary>
        public int Age;

        /// <summary>
        /// The gender of the patient.
        /// </summary>
        public Gender Gender;

        /// <summary>
        /// The position of the patient's jaw.
        /// </summary>
        public Vector3 JawPos;

        /// <summary>
        /// The position of the digue (dam) for the patient.
        /// </summary>
        public Vector3 DiguePos;
    }
}