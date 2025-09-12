using UnityEngine;

namespace ODON
{
    /// <summary>
    /// Manages the shader properties for the dam (digue) in the ODON application.
    /// Handles the position and activation of holes in the dam material based on the treated tooth.
    /// </summary>
    public class UpdateShaderDam : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// The tooth number to manage for the dam hole (must be between 11 and 48, excluding 19, 20, 29, 30, 39, 40).
        /// </summary>
        [Range(11, 48)]
        [SerializeField] private int teethToManage;

        /// <summary>
        /// Gets or sets the tooth number to manage for the dam hole.
        /// </summary>
        public int TeethToManage
        {
            get => teethToManage;
            set
            {
                if (value < 11 || value > 48)
                {
                    Debug.LogError("teethToManage must be between 11 and 48.");
                    return;
                }
                else if (value == 19 || value == 20
                       || value == 29 || value == 30
                       || value == 39 || value == 40)
                {
                    Debug.LogError("teethToManage cannot be 19, 20, 29, 30, 39, or 40.");
                    return;
                }
                teethToManage = value;
                GetHolePosition();
            }
        }

        /// <summary>
        /// Renderer for the dam material.
        /// </summary>
        [SerializeField] private Renderer damRenderer;

        /// <summary>
        /// Gets the renderer for the dam material.
        /// </summary>
        public Renderer DamRenderer => damRenderer;

        /// <summary>
        /// Array of possible hole positions for the dam.
        /// </summary>
        private readonly Vector2[] HolesPosition = new Vector2[] {
            new (0.47f, 0.14f),
            new (0.435f, 0.15f),
            new (0.393f, 0.185f),
            new (0.36f, 0.22f),
            new (0.339f, 0.26f),
            new (0.313f, 0.325f),
            new (0.295f, 0.40f),
            new (0.285f, 0.47f),
            new (0.705f, 0.475f),
            new (0.698f, 0.401f),
            new (0.675f, 0.328f),
            new (0.652f, 0.265f),
            new (0.628f, 0.225f),
            new (0.595f, 0.188f),
            new (0.548f, 0.153f),
            new (0.515f, 0.14f)
        };

        /// <summary>
        /// Current position of the hole in the dam material.
        /// </summary>
        [SerializeField] private Vector2 HolePosition = new ();

        /// <summary>
        /// Radius of the hole in the dam material.
        /// </summary>
        [SerializeField] private float HoleRadius = 0.1f;

        /// <summary>
        /// Falloff value for the hole edge in the dam material.
        /// </summary>
        [SerializeField] private float HoleFalloff = 0.01f;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods

        /// <summary>
        /// Initializes the dam shader properties and sets the hole position based on the treated tooth.
        /// </summary>
        void Start()
        {
            teethToManage = GameManager.HighlightsTeethManager.Instance.GoodTeethToDig.Id;

            if (damRenderer == null)
            {
                damRenderer.GetComponent<Renderer>();
                Debug.Log("Current Material used at runtime: " + damRenderer.material.name);
            }
            if (teethToManage <= 11 || teethToManage >= 48)
            {
                Debug.LogError("teethToManage must be between 11 and 48.");
                return;
            }
            else if (teethToManage == 19 || teethToManage == 20
                   || teethToManage == 29 || teethToManage == 30
                   || teethToManage == 39 || teethToManage == 40)
            {
                Debug.LogError("teethToManage cannot be 19, 20, 29, 30, 39, or 40.");
                return;
            }

            GetHolePosition();
            damRenderer.material.SetVector("_HolePosition", HolePosition);
            damRenderer.material.SetFloat("_HoleRadius", HoleRadius);
            damRenderer.material.SetFloat("_HoleFalloff", HoleFalloff);
            damRenderer.material.DisableKeyword("_ACTIVEHOLE");
        }

        /// <summary>
        /// Registers the SwitchActiveHole listener to the OnDigDam event.
        /// </summary>
        void OnEnable()
        {
            GameManager.HighlightsTeethManager.Instance.OnDigDam.AddListener(SwitchActiveHole);
        }

        /// <summary>
        /// Unregisters the SwitchActiveHole listener from the OnDigDam event.
        /// </summary>
        void OnDisable()
        {
            GameManager.HighlightsTeethManager.Instance.OnDigDam.RemoveListener(SwitchActiveHole);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        /// <summary>
        /// Enables or disables the active hole in the dam material based on the argument.
        /// </summary>
        /// <param name="isActive">If true, enables the hole; if false, disables it.</param>
        public void SwitchActiveHole(bool isActive)
        {
            if (isActive)
            {
                GetHolePosition();
                damRenderer.material.EnableKeyword("_ACTIVEHOLE");
                // Active cloth ? 
            }
            else
            {
                GetHolePosition();
                damRenderer.material.DisableKeyword("_ACTIVEHOLE");
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods

        /// <summary>
        /// Sets the hole position in the dam material based on the tooth number.
        /// </summary>
        private void GetHolePosition()
        {
            switch (teethToManage)
            {
                case 41: HolePosition = HolesPosition[0]; break;
                case 42: HolePosition = HolesPosition[1]; break;
                case 43: HolePosition = HolesPosition[2]; break;
                case 44: HolePosition = HolesPosition[3]; break;
                case 45: HolePosition = HolesPosition[4]; break;
                case 46: HolePosition = HolesPosition[5]; break;
                case 47: HolePosition = HolesPosition[6]; break;
                case 48: HolePosition = HolesPosition[7]; break;
                case 38: HolePosition = HolesPosition[8]; break;
                case 37: HolePosition = HolesPosition[9]; break;
                case 36: HolePosition = HolesPosition[10]; break;
                case 35: HolePosition = HolesPosition[11]; break;
                case 34: HolePosition = HolesPosition[12]; break;
                case 33: HolePosition = HolesPosition[13]; break;
                case 32: HolePosition = HolesPosition[14]; break;
                case 31: HolePosition = HolesPosition[15]; break;
                case 19:
                case 20:
                case 29:
                case 30:
                case 39:
                case 40:
                    Debug.LogError("teethToManage cannot be 19, 20, 29, 30, 39, or 40.");
                    break;
                default:
                    Debug.LogWarning($"HolePosition is not set for {teethToManage}");
                    break;
            }

            damRenderer.material.SetVector("_HolePosition", HolePosition);
            damRenderer.material.SetFloat("_HoleRadius", HoleRadius);
            damRenderer.material.SetFloat("_HoleFalloff", HoleFalloff);
        }
        #endregion
    }
}