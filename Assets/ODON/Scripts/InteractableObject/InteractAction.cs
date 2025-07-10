
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
        [SerializeField] protected GameObject gOToSwitchRendering;

        [Header("Target")]
        [SerializeField] protected Transform handTarget;
        [SerializeField] protected Data.E_HandNeed handNeed;

        #region Primary Fonction
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void OnHeadInteract()
        {
            throw new System.NotImplementedException();
        }

        #endregion
        #region Secondary Fonction
        
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnTakeInHand()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void DropAtInitPosition()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}