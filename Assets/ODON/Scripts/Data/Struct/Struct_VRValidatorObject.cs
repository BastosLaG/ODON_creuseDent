using JetBrains.Annotations;
using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    public class Struct_VRValidatorObject
    {
        [SerializeField] private GameObject ObjectInteractable;
        [SerializeField] private bool isValid = false;

        [SerializeField] private GameObject ObjectReplaceWhenValid;
        public GameObject ValidateObject => ObjectInteractable;
        public bool IsValid
        {
            get => isValid;
            set {
                isValid = value;
                ObjectInteractable.SetActive(!isValid);
                ObjectReplaceWhenValid.SetActive(isValid);
            }
        }
    }
}