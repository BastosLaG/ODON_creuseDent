using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    public class Struct_VRValidatorObject
    {
        [SerializeField] private GameObject valdateObject;
        [SerializeField] private bool isValid = false;
        public GameObject ValdateObject => valdateObject;
        public bool IsValid
        {
            get => isValid;
            set => isValid = value;
        }
    }
}