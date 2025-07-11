
using System.Collections.Generic;
using ODON.InteractableObject.Interface;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
        [SerializeField] protected List<Renderer> gORenderer = new();
        [SerializeField] protected List<Material> gOSavedMaterials = new();
        [SerializeField] protected Material transparentGrey;

        [Header("Target")]
        [SerializeField] protected Data.E_HandNeed handNeed;
        [SerializeField] protected Vector3 offsetPosition;
        [SerializeField] protected Vector3 offsetRotation;
        [SerializeField] protected bool isInteract = false;
        public bool IsInteract => isInteract;

        #region Init Methods
        void Awake()
        {
            gORenderer.AddRange(GetComponentsInChildren<Renderer>());
            foreach (Renderer r in gORenderer)
            {
                gOSavedMaterials.AddRange(r.materials);
            }
        }
        #endregion

        #region Primary Fonction
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void HeadInteract()
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
            instantiateObject.transform.SetLocalPositionAndRotation(offsetPosition, Quaternion.Euler(offsetRotation));

            Rigidbody rb = instantiateObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            foreach (Renderer renderer in gORenderer)
            {
                renderer.material = transparentGrey;
            }

            isInteract = true;
            
            if (choosenOne.childCount > 0)
            {
                choosenOne.GetChild(0).gameObject.SetActive(false);
            }
        }

        private void DropHandObject(Transform choosenOne)
        {
            Destroy(instantiateObject);
            
            foreach (Renderer renderer in gORenderer)
            {
                renderer.materials = gOSavedMaterials.ToArray();
            }

            isInteract = false;

            if (choosenOne.childCount > 0)
            {
                choosenOne.GetChild(0).gameObject.SetActive(true);
            }
        }

        #endregion
    }
}