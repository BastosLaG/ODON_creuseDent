using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// Serializable struct representing patient mouth data in the ODON system.
    /// Contains references to the patient body GameObject and patient metadata.
    /// </summary>
    [System.Serializable]
    public struct PatientMeta
    {
        /// <summary>
        /// Reference to the patient body GameObject.
        /// </summary>
        public GameObject patientBody;

        /// <summary>
        /// Metadata containing patient information.
        /// </summary>
        public SO_PatientMetaData patientMetaData;
    }
}