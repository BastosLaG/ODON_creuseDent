using UnityEditor;
using UnityEngine;

namespace ODON.Scripts.Digue
{
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
        [SerializeField] private Vector2 HolePosition = new Vector2(0.5f, 0.5f);
        [SerializeField] private float HoleRadius = 0.1f;
        [SerializeField] private float HoleFalloff = 0.01f;
        [SerializeField] private bool isHoleActive = false;

        void Start()
        {
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
            else
            {
                Debug.LogWarning("HolePosition is not set for this number of teeth");
            }
        }

    }
}