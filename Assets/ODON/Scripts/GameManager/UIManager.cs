using UnityEngine;
using UnityEngine.Events;

namespace ODON.GameManager
{

    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [SerializeField] private Data.PatientData patientData;
        public UnityEvent<Data.PatientData> onUpdatePatientData = new();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }  

        public void UpdatePatientData()
        {
            onUpdatePatientData.Invoke(patientData);
        }
    }
}