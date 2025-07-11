
using System.Collections.Generic;
using ODON.InteractableObject.Interface;
using UnityEngine;

namespace ODON.InteractableObject
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class InteractAction : MonoBehaviour, IInteractWithHeadInteractor
    {

        [Header("GameObjects")]
        [SerializeField] protected GameObject gOToInstanciate;
        private GameObject instantiateObject;
        [SerializeField] protected Renderer gORenderer;
        [SerializeField] protected Material transparentGrey;
        [SerializeField] protected List<Material> gOSavedMaterials = new();



        [Header("Target")]
        [SerializeField] protected Data.E_HandNeed handNeed;

        [SerializeField] protected bool isInteract = false;
        public bool IsInteract => isInteract;

        #region Init Methods

        void Awake()
        {
            if (TryGetComponent<Renderer>(out gORenderer))
            {
                gOSavedMaterials.AddRange(gORenderer.materials);
            }
        }

        #endregion

        #region Primary Fonction
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void OnHeadInteract()
        {
            Debug.LogWarning("Base InteractAction.OnHeadInteract called");
            throw new System.NotImplementedException();
        }
        #endregion
        #region Secondary Fonction

        protected void SwapToHand()
        {
            if (!isInteract)
            {
                switch (handNeed)
                {
                    case Data.E_HandNeed.None:
                        break;
                    case Data.E_HandNeed.Left:
                        TakeInHand(GameManager.GameHandler.Instance.LeftHand);
                        break;
                    case Data.E_HandNeed.Right:
                        TakeInHand(GameManager.GameHandler.Instance.RightHand);
                        break;
                    case Data.E_HandNeed.Both:
                        //TODO check si une main est libre prioriser la main gauche.
                        break;
                    default:
                        Debug.LogWarning($"SwapToHand / Incorrect value {handNeed}");
                        break;
                }
            }
            else
            {
                switch (handNeed)
                {
                    case Data.E_HandNeed.None:
                        break;
                    case Data.E_HandNeed.Left:
                        DropHandObject(GameManager.GameHandler.Instance.LeftHand);
                        break;
                    case Data.E_HandNeed.Right:
                        DropHandObject(GameManager.GameHandler.Instance.RightHand);
                        break;
                    case Data.E_HandNeed.Both:
                        //TODO check si une main est libre prioriser la main gauche.
                        break;
                    default:
                        Debug.LogWarning($"SwapToHand / Incorrect value {handNeed}");
                        break;
                }
            }
        }

        private void TakeInHand(Transform choosenOne)
        {
            instantiateObject = Instantiate(gOToInstanciate, choosenOne);
            instantiateObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f));
            Debug.Log($"instantiate object name {instantiateObject.name}, pos {instantiateObject.transform.position}, rot {instantiateObject.transform.rotation}");
            gORenderer.material = transparentGrey;
            isInteract = true;
            choosenOne.GetChild(0).gameObject.SetActive(false);
        }

        private void DropHandObject(Transform choosenOne)
        {
            Destroy(instantiateObject);
            gORenderer.materials = gOSavedMaterials.ToArray();
            isInteract = false;
            choosenOne.GetChild(0).gameObject.SetActive(true);
        }
        #endregion
    }
}