using ODON.GameManager;
using UnityEngine;

namespace ODON
{
    public class UpdateShaderDam : MonoBehaviour
    {
        #region Properities
        [Range(11, 48)]
        [SerializeField] private int teethToManage;
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
        [SerializeField] private Renderer damRenderer;
        public Renderer DamRenderer => damRenderer;
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
        [SerializeField] private Vector2 HolePosition = new ();
        [SerializeField] private float HoleRadius = 0.1f;
        [SerializeField] private float HoleFalloff = 0.01f;
        [SerializeField] private Transform pliersTransform = null;
        [SerializeField] private SetObjectGrabable transformDamGrabble;

        [SerializeField] private HighlightsTeethManager highlightsTeethManager;
        [SerializeField] private UniversalSenderActionToEventManager uSATEManager;
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods
        void Start()
        {
            if (pliersTransform == null)
            {
                Debug.LogError("Pliers Transform is not assigned.");
                return;
            }
            if (damRenderer == null)
            {
                damRenderer.GetComponent<Renderer>();
                Debug.Log("Current Material used at runtime: " + damRenderer.material.name);
            }
            if (transformDamGrabble == null)
            {
                Debug.LogError("TransformDam is not assigned.");
                return;
            }
            if (teethToManage < 11 || teethToManage > 48)
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
            SwitchActiveHole(false);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        public void SwitchActiveHole(bool isActive)
        {
            if (isActive)
            {
                uSATEManager.TryValdidateCurrentItem();
                damRenderer.material.EnableKeyword("_ACTIVEHOLE");
            }
            else
            {
                uSATEManager.TryValdidateCurrentItem(false);
                damRenderer.material.DisableKeyword("_ACTIVEHOLE");
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods

        private void GetHolePosition()
        {
            if (teethToManage == 41) HolePosition = HolesPosition[0];
            else if (teethToManage == 42) HolePosition = HolesPosition[1];
            else if (teethToManage == 43) HolePosition = HolesPosition[2];
            else if (teethToManage == 44) HolePosition = HolesPosition[3];
            else if (teethToManage == 45) HolePosition = HolesPosition[4];
            else if (teethToManage == 46) HolePosition = HolesPosition[5];
            else if (teethToManage == 47) HolePosition = HolesPosition[6];
            else if (teethToManage == 48) HolePosition = HolesPosition[7];
            else if (teethToManage == 38) HolePosition = HolesPosition[8];
            else if (teethToManage == 37) HolePosition = HolesPosition[9];
            else if (teethToManage == 36) HolePosition = HolesPosition[10];
            else if (teethToManage == 35) HolePosition = HolesPosition[11];
            else if (teethToManage == 34) HolePosition = HolesPosition[12];
            else if (teethToManage == 33) HolePosition = HolesPosition[13];
            else if (teethToManage == 32) HolePosition = HolesPosition[14];
            else if (teethToManage == 31) HolePosition = HolesPosition[15];
            else if (teethToManage == 19 || teethToManage == 20
                || teethToManage == 29 || teethToManage == 30
                || teethToManage == 39 || teethToManage == 40)
            {
                Debug.LogError("teethToManage cannot be 19, 20, 29, 30, 39, or 40.");
            }
            else
            {
                Debug.LogWarning($"HolePosition is not set for {teethToManage}");
            }

            damRenderer.material.SetVector("_HolePosition", HolePosition);
            damRenderer.material.SetFloat("_HoleRadius", HoleRadius);
            damRenderer.material.SetFloat("_HoleFalloff", HoleFalloff);
        }
        #endregion
    }
}