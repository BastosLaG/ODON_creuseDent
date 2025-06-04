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

        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private GameObject tablet;
        [SerializeField] private GameObject clipBoard;
        [SerializeField] private GameObject pliersBrewer;
        [SerializeField] private GameObject pliersAinsworth;
        [SerializeField] private GameObject crampon;
        [SerializeField] private GameObject cadreEnU;
        [SerializeField] private GameObject dents;
        [SerializeField] private GameObject supportDigue;
        public GameObject Tablet
        {
            get => tablet;
            private set => tablet = value;
        }
        public GameObject ClipBoard
        {
            get => clipBoard;
            private set => clipBoard = value;
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
        public GameObject Dents
        {
            get => dents;
            private set => dents = value;
        }
        public GameObject SupportDigue
        {
            get => supportDigue;
            private set => supportDigue = value;
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
    }
}