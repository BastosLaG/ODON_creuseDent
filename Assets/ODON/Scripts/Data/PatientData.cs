using UnityEditor.EditorTools;
using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PatientData", menuName = "ScriptableObjects/PatientData", order = 1)]
    public class PatientData : ScriptableObject
    {
        [Header("Patient Information")]
        [Tooltip("Name of the patient.")]
        public string patientName;

        [Tooltip("Age of the patient.")]
        [Range(1, 120)]
        public int age;
        
        [Tooltip("TeintedThooth object representing the tooth condition of the patient.")]
        public TeintedThooth teintedTooth;

        [Tooltip("Gender of the patient.")]
        public Gender gender;

        [Tooltip("Thooth section of the patient that was operated on.")]
        [Range(1, 4)]
        public int toothSection;

        [Tooltip("Tooth that was operated on.")]
        [Range(1, 8)]
        public int treatedTooth;
        public int treatedToothWithSection => (toothSection * 10) + treatedTooth;

        [Tooltip("Indicates if the patient has a latex allergy.")]
        public bool hasLatexAllergy;

        [Header("Type of pose")]
        public bool hasNormalPose;

        [Header("Prosthetic Information")]
        [Tooltip("Indicates if an inlay-core is used.")]
        public bool inlayCore;

        [Tooltip("Indicates if a cast crown is used.")]
        public bool castCrown;

        [Tooltip("Indicates if a metal-ceramic crown (CCM) is used.")]
        public bool metalCeramicCrown;

        [Tooltip("Indicates if a stellite prosthesis is used.")]
        public bool stellite;

        [Tooltip("Indicates if a resin partial denture is used.")]
        public bool resinPartial;

        [Tooltip("Indicates if a zirconia prosthesis is used.")]
        public bool zirconia;
    }
}
