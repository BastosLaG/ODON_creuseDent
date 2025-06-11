using System.ComponentModel;
using ODON.Data;
using UnityEngine;

namespace ODON.GameManager
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler instance;
        public static GameHandler Instance => instance;

        [Header("Security Items")]
        [Tooltip("List of security items.")]
        [SerializeField] private bool isBlouseWear;
        [SerializeField] private bool isGlovesWear;
        [SerializeField] private bool isGlassesWear;
        public bool IsBlouseWear
        {
            get => isBlouseWear;
            set => isBlouseWear = value;
        }
        public bool IsGlovesWear
        {
            get => isGlovesWear;
            set => isGlovesWear = value;
        }
        public bool IsGlassesWear
        {
            get => isGlassesWear;
            set => isGlassesWear = value;
        }

        [Header("Interactive items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private GameObject tablet;
        [SerializeField] private GameObject door;
        [SerializeField] private GameObject clipBoard;
        [SerializeField] private GameObject securityEquipment;
        [SerializeField] private GameObject pliersBrewer;
        [SerializeField] private GameObject pliersAinsworth;
        [SerializeField] private GameObject crampon;
        [SerializeField] private GameObject cadreEnU;
        [SerializeField] private GameObject lowerDenture;
        [SerializeField] private GameObject supportDigue;
        [SerializeField] private GameObject digue;
        [SerializeField] private GameObject dentalFloss;
        public GameObject Tablet
        {
            get => tablet;
            private set => tablet = value;
        }
        public GameObject Door
        {
            get => door;
            private set => door = value;
        }
        public GameObject ClipBoard
        {
            get => clipBoard;
            private set => clipBoard = value;
        }
        public GameObject SecurityEquipment
        {
            get => securityEquipment;
            private set => securityEquipment = value;
        }
        public GameObject PliersBrewer
        {
            get => pliersBrewer;
            private set => pliersBrewer = value;
        }
        public GameObject PliersAinsworth
        {
            get => pliersAinsworth;
            private set => pliersAinsworth = value;
        }
        public GameObject Crampon
        {
            get => crampon;
            private set => crampon = value;
        }
        public GameObject CadreEnU
        {
            get => cadreEnU;
            private set => cadreEnU = value;
        }
        public GameObject LowerDenture
        {
            get => lowerDenture;
            private set => lowerDenture = value;
        }
        public GameObject SupportDigue
        {
            get => supportDigue;
            private set => supportDigue = value;
        }
        public GameObject Digue
        {
            get => digue;
            private set => digue = value;
        }
        public GameObject DentalFloss
        {
            get => dentalFloss;
            private set => dentalFloss = value;
        }

        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private HighlightableItem[] highlightableItems;
        [SerializeField] private int currentHighlightableIndex = 0;
        [SerializeField] private int techniqueId = 0;
        public HighlightableItem[] HighlightableItems
        {
            get => highlightableItems;
            set => highlightableItems = value;
        }
        public int CurrentHighlightableIndex
        {
            get => currentHighlightableIndex;
            set => currentHighlightableIndex = value;
        }
        public int TechniqueId
        {
            get => techniqueId;
            set => techniqueId = value;
        }

        [Header("Patient Data")]
        [SerializeField] private PatientData[] patientData;
        [SerializeField] private int patientDataIndex = 0;
        [SerializeField] private PatientState patientState = PatientState.InWaitingRoom;
        [SerializeField] private bool isPatientInRoom = false; 
        [SerializeField] private bool isPatientInBed = false; 
        public PatientData[] PatientData
        {
            get => patientData;
            private set => patientData = value;
        }
        public int PatientDataIndex
        {
            get => patientDataIndex;
            private set => patientDataIndex = value;
        }
        public PatientState PatientState
        {
            get => patientState;
            set => patientState = value;
        }

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

        void Start()
        {
            HighlightsManager.Instance.InitHighLight();
            UIManager.Instance.InitClipBoard();
        }

        public void NewPatientEnterOnRoom()
        {
            if (!isPatientInRoom)
            {
                patientState = PatientState.InCabinet;
                isPatientInRoom = true;
            }
        }

        public void PatientSitOnBed()
        {
            if (!isPatientInBed)
            {
                isPatientInBed = true;
                patientState = PatientState.InBed;
            }
        }
        
        public void PatientExitFromRoom()
        {
            patientState = PatientState.InWaitingRoom;
            // patientDataIndex++;
            // if (patientDataIndex >= patientData.Length)
            // {
            //     patientDataIndex = 0;
            // }
        }

    }
}