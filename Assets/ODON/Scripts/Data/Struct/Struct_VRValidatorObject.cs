using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    public class Struct_VRValidatorObject
    {
        [SerializeField] private GameObject validateObject;
        [SerializeField] private bool isValid = false;
        public GameObject ValidateObject => validateObject;
        public bool IsValid
        {
            get => isValid;
            set => isValid = value;
        }
    }
}