using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

namespace ODON.GameManager
{
    public class GameHandler : MonoBehaviour
    {
        #region Singleton
        /// <summary>
        /// Singleton instance of GameHandler.
        /// </summary>
        /// <remarks>
        /// This class manages the game state, including patient data, interactive items, and security items.
        /// It ensures that only one instance of GameHandler exists throughout the game.
        private static GameHandler instance;
        public static GameHandler Instance => instance;
        #endregion
        #region Scenario
        [Header("Scenario")]
        [Tooltip("The scenario data for the game.")]
        [SerializeField] private List<Data.SO_Scenario> scenario;
        #endregion
        #region Security Items
        [Header("Security Items")]
        [Tooltip("List of security items.")]
        [SerializeField] private bool isBlouseWear;
        [SerializeField] private bool isGlovesWear;
        [SerializeField] private bool isGlassesWear;
        #endregion
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

        #region Interactive Items
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

        // [SerializeField] private ISendActiveCheckpointProgress[] interactiveItems;

        #endregion
        #region Properties Setters/Getters
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
        #endregion

        #region Highlightable Items
        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private Data.HighlightableItem[] highlightableItems;
        [SerializeField] private int currentHighlightableIndex = 0;
        [SerializeField] private int techniqueId = 0;
        public Data.HighlightableItem[] HighlightableItems
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
        #endregion

        #region Patient Data
        [Header("Patient Data")]
        [SerializeField] private Data.PatientData[] patientData;
        [SerializeField] private int patientDataIndex = 0;
        [SerializeField] private Data.PatientState patientState = Data.PatientState.InWaitingRoom;
        [SerializeField] private bool isPatientInRoom = false;
        [SerializeField] private bool isPatientInBed = false;
        public Data.PatientData[] PatientData
        {
            get => patientData;
            private set => patientData = value;
        }
        public int PatientDataIndex
        {
            get => patientDataIndex;
            private set => patientDataIndex = value;
        }
        public Data.PatientState PatientState
        {
            get => patientState;
            set => patientState = value;
        }

        #endregion
        #region Initialization
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
            // GetSequenceForTechnique(TechniqueId);
            HighlightsManager.Instance.InitHighLight();
            UIManager.Instance.InitClipBoard();
        }

        #endregion

        void Update()
        {
            // if (interactiveItems[currentHighlightableIndex] != null)
            // {
            //     foreach (var item in interactiveItems)
            //     {
            //         if (item != null && item.IsActiveCheckpointProgressEnabled && !item.IsLocked)
            //         {
            //             HighlightsManager.Instance.SwitchActiveItem(1);
            //             item.IsLocked = true;
            //         }
            //     }
            // }
        }

        #region Patient Management
        public void NewPatientEnterOnRoom()
        {
            if (!isPatientInRoom)
            {
                patientState = Data.PatientState.InCabinet;
                isPatientInRoom = true;
            }
        }

        public void PatientSitOnBed()
        {
            if (!isPatientInBed)
            {
                isPatientInBed = true;
                patientState = Data.PatientState.InBed;
            }
        }

        public void PatientExitFromRoom()
        {
            patientState = Data.PatientState.InWaitingRoom;
        }
        #endregion
        #region Interactive Items
        // private ISendActiveCheckpointProgress[] GetSequenceForTechnique(int techniqueId)
        // {
        //     switch (techniqueId)
        //     {
        //         case 0: // Pose classic
        //             return new ISendActiveCheckpointProgress[]
        //             {
        //                 Tablet.GetComponent<ISendActiveCheckpointProgress>(),
        //                 Door.GetComponent<ISendActiveCheckpointProgress>(),
        //                 ClipBoard.GetComponent<ISendActiveCheckpointProgress>(),
        //                 SecurityEquipment.GetComponent<ISendActiveCheckpointProgress>(),
        //                 SupportDigue.GetComponent<ISendActiveCheckpointProgress>(),
        //                 PliersAinsworth.GetComponent<ISendActiveCheckpointProgress>(),
        //                 PliersBrewer.GetComponent<ISendActiveCheckpointProgress>(),
        //                 Crampon.GetComponent<ISendActiveCheckpointProgress>(),
        //                 Digue.GetComponent<ISendActiveCheckpointProgress>(),
        //                 LowerDenture.GetComponent<ISendActiveCheckpointProgress>(),
        //                 CadreEnU.GetComponent<ISendActiveCheckpointProgress>(),
        //                 DentalFloss.GetComponent<ISendActiveCheckpointProgress>()
        //             };

        //         case 1: // Pose parachute
        //             return new ISendActiveCheckpointProgress[]
        //             {
        //                 Tablet.GetComponent<ISendActiveCheckpointProgress>(),
        //                 Door.GetComponent<ISendActiveCheckpointProgress>(),
        //                 ClipBoard.GetComponent<ISendActiveCheckpointProgress>()
        //             };

        //         default:
        //             Debug.LogWarning("Technique ID unknown.");
        //             return new ISendActiveCheckpointProgress[0];
        //     }
        // }
        #endregion
    }
}