using System.ComponentModel;
using ODON.Data;
using UnityEngine;

namespace ODON.GameManager
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler instance;
        public static GameHandler Instance => instance;

        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private GameObject tablet;
        [SerializeField] private GameObject clipBoard;
        [SerializeField] private GameObject pinceBrewer;
        [SerializeField] private GameObject pinceAinsworth;
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
        public GameObject PinceBrewer
        {
            get => pinceBrewer;
            private set => pinceBrewer = value;
        }
        public GameObject PinceAinsworth
        {
            get => pinceAinsworth;
            private set => pinceAinsworth = value;
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
            HighlightsManager.Instance.InitHighLight();
            UIManager.Instance.InitClipBoard();
        }
    }
}