using System;
using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// ScriptableObject representing patient data in the ODON system.
    /// Stores patient information, tooth condition, allergies, and prosthetic details.
    /// </summary>
    [CreateAssetMenu(fileName = "PatientData", menuName = "ScriptableObjects/PatientData", order = 1)]
    public class PatientData : ScriptableObject
    {
        /// <summary>
        /// Name of the patient.
        /// </summary>
        [Header("Patient Information")]
        [Tooltip("Name of the patient.")]
        public string PatientName
        {
            get => PatientMetaData.PatientName.ToString();
            set => PatientMetaData.PatientName = Enum.Parse<PatientNames>(value);
        }

        /// <summary>
        /// Age of the patient.
        /// </summary>
        [Tooltip("Age of the patient.")]
        [Range(1, 120)]
        public int Age
        {
            get => PatientMetaData.Age;
            set => PatientMetaData.Age = value;
        }

        /// <summary>
        /// Gender of the patient.
        /// </summary>
        [Tooltip("Gender of the patient.")]
        public Gender Gender
        {
            get => PatientMetaData.Gender;
            set => PatientMetaData.Gender = value;
        }

        /// <summary>
        /// Metadata containing patient information.
        /// </summary>
        public SO_PatientMetaData PatientMetaData;

        /// <summary>
        /// TeintedThooth object representing the tooth condition of the patient.
        /// </summary>
        [Tooltip("TeintedThooth object representing the tooth condition of the patient.")]
        public TeintedThooth TeintedTooth;

        /// <summary>
        /// Tooth section of the patient that was operated on.
        /// </summary>
        [Tooltip("Tooth section of the patient that was operated on.")]
        [Range(1, 4)]
        public int ToothSection;

        /// <summary>
        /// Tooth that was operated on.
        /// </summary>
        [Tooltip("Tooth that was operated on.")]
        [Range(1, 8)]
        public int TreatedTooth;

        /// <summary>
        /// Gets the treated tooth with section as a combined integer.
        /// </summary>
        public int TreatedToothWithSection => (ToothSection * 10) + TreatedTooth;

        /// <summary>
        /// Indicates if the patient has a latex allergy.
        /// </summary>
        [Tooltip("Indicates if the patient has a latex allergy.")]
        public bool HasLatexAllergy;

        /// <summary>
        /// Indicates if the patient has a normal pose.
        /// </summary>
        [Header("Type of pose")]
        public bool HasNormalPose;

        /// <summary>
        /// Indicates if an inlay-core is used.
        /// </summary>
        [Header("Prosthetic Information")]
        [Tooltip("Indicates if an inlay-core is used.")]
        public bool InlayCore;

        /// <summary>
        /// Indicates if a cast crown is used.
        /// </summary>
        [Tooltip("Indicates if a cast crown is used.")]
        public bool CastCrown;

        /// <summary>
        /// Indicates if a metal-ceramic crown (CCM) is used.
        /// </summary>
        [Tooltip("Indicates if a metal-ceramic crown (CCM) is used.")]
        public bool MetalCeramicCrown;

        /// <summary>
        /// Indicates if a stellite prosthesis is used.
        /// </summary>
        [Tooltip("Indicates if a stellite prosthesis is used.")]
        public bool Stellite;

        /// <summary>
        /// Indicates if a resin partial denture is used.
        /// </summary>
        [Tooltip("Indicates if a resin partial denture is used.")]
        public bool ResinPartial;

        /// <summary>
        /// Indicates if a zirconia prosthesis is used.
        /// </summary>
        [Tooltip("Indicates if a zirconia prosthesis is used.")]
        public bool Zirconia;
    }
}
