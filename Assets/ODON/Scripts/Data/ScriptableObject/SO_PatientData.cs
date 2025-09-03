using UnityEngine;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "PatientData", menuName = "ScriptableObjects/PatientData", order = 1)]
    public class PatientData : ScriptableObject
    {
        [Header("Patient Information")]
        [Tooltip("Name of the patient.")]
        public string PatientName;

        [Tooltip("Age of the patient.")]
        [Range(1, 120)]
        public int Age;

        [Tooltip("TeintedThooth object representing the tooth condition of the patient.")]
        public TeintedThooth TeintedTooth;

        [Tooltip("Gender of the patient.")]
        public Gender Gender;

        [Tooltip("Tooth section of the patient that was operated on.")]
        [Range(1, 4)]
        public int ToothSection;

        [Tooltip("Tooth that was operated on.")]
        [Range(1, 8)]
        public int TreatedTooth;
        public int TreatedToothWithSection => (ToothSection * 10) + TreatedTooth;

        [Tooltip("Indicates if the patient has a latex allergy.")]
        public bool HasLatexAllergy;

        [Header("Type of pose")]
        public bool HasNormalPose;

        [Header("Prosthetic Information")]
        [Tooltip("Indicates if an inlay-core is used.")]
        public bool InlayCore;

        [Tooltip("Indicates if a cast crown is used.")]
        public bool CastCrown;

        [Tooltip("Indicates if a metal-ceramic crown (CCM) is used.")]
        public bool MetalCeramicCrown;

        [Tooltip("Indicates if a stellite prosthesis is used.")]
        public bool Stellite;

        [Tooltip("Indicates if a resin partial denture is used.")]
        public bool ResinPartial;

        [Tooltip("Indicates if a zirconia prosthesis is used.")]
        public bool Zirconia;


        public void LoadMetaData(string name, int age, Gender gender)
        {
            PatientName = name;
            Age = age;
            Gender = gender;
        }
    }
}
