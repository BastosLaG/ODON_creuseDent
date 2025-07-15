
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
        [Header("GameObject we want to instantiate in the hand")]
        /// <summary>
        /// The GameObject to instantiate when the interaction occurs.
        /// </summary>
        /// <remarks>
        /// This GameObject will be instantiated in the player's hand when the interaction is triggered.
        /// </remarks>
        [SerializeField] protected GameObject gOToInstantiate = null;
        /// <summary>
        /// The instantiated GameObject in the player's hand.
        /// </summary>
        /// <remarks>
        /// This GameObject is the result of the instantiation of gOToInstantiate in the player's hand.
        /// </remarks>
        private GameObject instantiateObject;
        /// <summary>
        /// The list of renderers associated with the GameObject.
        /// </summary>
        [SerializeField] protected List<Renderer> gORenderer = new();
        /// <summary>   
        /// The list of saved materials for the GameObject.
        /// </summary>
        [SerializeField] protected List<Material> gOSavedMaterials = new();

        /// <summary>
        /// The material used to indicate that the object is take by the player.
        /// </summary>
        /// <remarks>
        /// This material is applied to the GameObject when it is instantiated in the player's hand.
        /// </remarks>
        [Header("Material")]
        [SerializeField] protected Material transparentGrey;

        [Header("Target")]
        [SerializeField] protected Data.E_HandNeed handNeed;
        [SerializeField] protected Vector3 offsetPosition;
        [SerializeField] protected Vector3 offsetRotation;

        [Header("CheckBox to know if the object is in the hand")]
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

        #region Primary Function
        public virtual void HeadInteract()
        {
            Debug.LogWarning("Base InteractAction.OnHeadInteract called");
            throw new System.NotImplementedException();
        }
        public virtual void HeadHoverEventBegin()
        {
            Debug.LogWarning("Base InteractAction.OnHeadHoverEventBegin called");
            throw new System.NotImplementedException();
        }
        public virtual void HeadHoverEventEnd()
        {
            Debug.LogWarning("Base InteractAction.OnHeadHoverEventEnd called");
            throw new System.NotImplementedException();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////

        #region Secondary Function
        protected void SwapToHand()
        {
            if (!isInteract)
            {
                switch (handNeed)
                {
                    case Data.E_HandNeed.None:
                        break;
                    case Data.E_HandNeed.Left:
                        if (GameManager.GameHandler.Instance.LeftHand.gameObject.activeInHierarchy)
                        {
                            TakeInHand(GameManager.GameHandler.Instance.LeftHand);
                        }
                        else
                        {
                            Debug.LogWarning($"You try to take : {gOToInstantiate.name} while your Left hand is occupied");
                        }
                        break;
                    case Data.E_HandNeed.Right:
                        if (GameManager.GameHandler.Instance.RightHand.gameObject.activeInHierarchy)
                        {
                            TakeInHand(GameManager.GameHandler.Instance.RightHand);
                        }
                        else
                        {
                            Debug.LogWarning($"You try to take : {gOToInstantiate.name} while your Right hand is occupied");
                        }
                        break;
                    case Data.E_HandNeed.Both:
                        if (GameManager.GameHandler.Instance.LeftHand.gameObject.activeInHierarchy)
                        {
                            TakeInHand(GameManager.GameHandler.Instance.LeftHand);
                        }
                        else if (GameManager.GameHandler.Instance.RightHand.gameObject.activeInHierarchy)
                        {
                            TakeInHand(GameManager.GameHandler.Instance.RightHand);
                        }
                        else
                        {
                            Debug.LogWarning($"You try to take : {gOToInstantiate.name} while your both hands are occupied");
                            // Todo - Implement UI to inform the player of the situation of this hand
                        }
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
                        if (GameManager.GameHandler.Instance.LeftHand.gameObject.activeInHierarchy == instantiateObject)
                        {
                            DropHandObject(GameManager.GameHandler.Instance.LeftHand, instantiateObject);
                        }
                        else if (GameManager.GameHandler.Instance.RightHand.gameObject.activeInHierarchy == instantiateObject)
                        {
                            DropHandObject(GameManager.GameHandler.Instance.RightHand, instantiateObject);
                        }
                        else
                        {
                            Debug.LogError($"the object : {instantiateObject.name} is not in the hands");
                        }
                        break;
                    default:
                        Debug.LogWarning($"SwapToHand / Incorrect value {handNeed}");
                        break;
                }
            }
        }

        protected virtual void TakeInHand(Transform chosenHand)
        {
            if (gOToInstantiate == null)
            {
                return;
            }

            foreach (Transform child in chosenHand)
            {
                if (child.gameObject.activeInHierarchy)
                {
                    child.gameObject.SetActive(false);
                }
            }

            instantiateObject = Instantiate(gOToInstantiate, chosenHand);
            instantiateObject.transform.SetLocalPositionAndRotation(offsetPosition, Quaternion.Euler(offsetRotation));

            if (instantiateObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            foreach (Renderer renderer in gORenderer)
            {
                renderer.material = transparentGrey;
            }

            isInteract = true;
        }

        protected virtual void DropHandObject(Transform chosenHand, GameObject instantiateObject = null)
        {
            // Foreach child in the chosen hand, find the instantiated object
            int indexInstantiateObject = 0;
            foreach (Transform child in chosenHand)
            {
                if (child.gameObject == instantiateObject)
                {
                    break;
                }
                indexInstantiateObject++;
            }
            
            Destroy(instantiateObject);
            
            // Restore the original materials
            foreach (Renderer renderer in gORenderer)
            {
                renderer.materials = gOSavedMaterials.ToArray();
            }
            // Set the interact flags to false
            isInteract = false;
            
            // Activate the last child in the chosen Hand && Condition the last child are not the hand
            if (chosenHand.childCount > 0 && indexInstantiateObject > 0)
            {
                chosenHand.GetChild(indexInstantiateObject - 1).gameObject.SetActive(true);
            }
        }

        #endregion
    }
}