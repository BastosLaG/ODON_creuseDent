using UnityEditor;
using UnityEngine;

namespace ODON.Scripts.Digue
{
    [RequireComponent(typeof(Cloth))]
    public class UpdateShaderDam : MonoBehaviour
    {
        [Range(11, 48)]
        [SerializeField] private int teethToManage = 11;
        [SerializeField] private Material damMaterial;
        public Material DamMaterial
        {
            get
            {
                if (damMaterial == null)
                {
                    damMaterial = GetComponent<Renderer>().material;
                }
                return damMaterial;
            }
        }
        [SerializeField] private Cloth cloth;
        public Cloth Cloth
        {
            get
            {
                if (cloth == null)
                {
                    cloth = GetComponent<Cloth>();
                }
                return cloth;
            }
        }
        [SerializeField] private Vector2 HolePosition = new Vector2(0.5f, 0.5f);
        [SerializeField] private float HoleRadius = 0.1f;
        [SerializeField] private float HoleFalloff = 0.01f;
        [SerializeField] private bool isHoleActive = false;

        [SerializeField] private Transform pliersTransform = null;
        [SerializeField] private SetObjectGrabable transformDamGrabable;

        void Start()
        {
            if (pliersTransform == null)
            {
                Debug.LogError("Pliers Transform is not assigned.");
                return;
            }
            damMaterial = DamMaterial ?? GetComponent<Renderer>().material;
            if (damMaterial == null)
            {
                Debug.LogError("DamMaterial is not assigned or found.");
                return;
            }
            cloth = Cloth ?? GetComponent<Cloth>();
            if (cloth == null)
            {
                Debug.LogError("Cloth component is not assigned or found.");
                return;
            }
            if (transformDamGrabable == null)
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

            DamMaterial.SetVector("_HolePosition", HolePosition);
            DamMaterial.SetFloat("_HoleRadius", HoleRadius);
            DamMaterial.SetFloat("_HoleFalloff", HoleFalloff);
            DamMaterial.SetInt("_IsHoleActive", isHoleActive ? 1 : 0);
        }

        private void FixedUpdate()
        {
            if (DamMaterial == null)
            {
                Debug.LogError("DamMaterial is not assigned or found.");
                return;
            }

            GetHolePosition();

            DamMaterial.SetVector("_HolePosition", HolePosition);
            DamMaterial.SetFloat("_HoleRadius", HoleRadius);
            DamMaterial.SetFloat("_HoleFalloff", HoleFalloff);
            DamMaterial.SetInt("_IsHoleActive", isHoleActive ? 1 : 0);
        }

        private void GetHolePosition()
        {
            if (teethToManage == 41) HolePosition = new Vector2(0.47f, 0.14f);
            else if (teethToManage == 42) HolePosition = new Vector2(0.435f, 0.15f);
            else if (teethToManage == 43) HolePosition = new Vector2(0.393f, 0.185f);
            else if (teethToManage == 44) HolePosition = new Vector2(0.36f, 0.22f);
            else if (teethToManage == 45) HolePosition = new Vector2(0.339f, 0.26f);
            else if (teethToManage == 46) HolePosition = new Vector2(0.313f, 0.325f);
            else if (teethToManage == 47) HolePosition = new Vector2(0.295f, 0.40f);
            else if (teethToManage == 48) HolePosition = new Vector2(0.285f, 0.47f);
            else if (teethToManage == 38) HolePosition = new Vector2(0.705f, 0.475f);
            else if (teethToManage == 37) HolePosition = new Vector2(0.698f, 0.401f);
            else if (teethToManage == 36) HolePosition = new Vector2(0.675f, 0.328f);
            else if (teethToManage == 35) HolePosition = new Vector2(0.652f, 0.265f);
            else if (teethToManage == 34) HolePosition = new Vector2(0.628f, 0.225f);
            else if (teethToManage == 33) HolePosition = new Vector2(0.595f, 0.188f);
            else if (teethToManage == 32) HolePosition = new Vector2(0.548f, 0.153f);
            else if (teethToManage == 31) HolePosition = new Vector2(0.515f, 0.14f);
            else if (teethToManage == 19 || teethToManage == 20
                   || teethToManage == 29 || teethToManage == 30
                   || teethToManage == 39 || teethToManage == 40)
            {
                Debug.LogError("teethToManage cannot be 19, 20, 29, 30, 39, or 40.");
            }
            else
            {
                Debug.LogWarning("HolePosition is not set for this number of teeth");
            }
        }

        public void SetHoleActive(bool isActive)
        {
            float distance = Vector3.Distance(transform.position, pliersTransform.position);
            if (distance >= 0.2f)
            {
                Debug.LogWarning("Pliers are too far from the dam to activate the hole. distance = " + distance);
                return;
            }
            isHoleActive = isActive;
            cloth.enabled = isActive;
            transformDamGrabable.enabled = isActive;
            GetHolePosition();
            DamMaterial.SetInt("_IsHoleActive", isHoleActive ? 1 : 0);
        }
    }
}