using System;
using UnityEngine;


namespace ODON.GameManager
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private Data.SO_Scenario scenario;
        [SerializeField] private int eventManagerId = 0;
        public int EventManagerId => eventManagerId;

        public static EventManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// Event triggered when an action is correctly passed.
        /// /// </summary>
        /// <remarks>
        /// This event is used to notify when an action has been successfully completed.
        /// </remarks>
        /// <param name="id">The ID of the action that was passed.</param>
        /// <param name="isCorrect">Indicates whether the action was passed correctly or not.</param>
        /// <param name="description">A description of the action that was passed for the Log.</param> 
        public event Action<Data.E_NameActionInteractable, bool, string> OnActionPassed;
        public void ActionCorrectlyPassed(Data.E_NameActionInteractable id, bool isCorrect = true, string description = "")
        {
            OnActionPassed?.Invoke(id, isCorrect, description);
        }

        /// <summary>
        /// Event triggered when an action is not correctly passed.
        /// </summary>
        /// <remarks>
        /// This event is used to notify when an action has failed or was not completed correctly.
        /// </remarks>
        /// <param name="id">The ID of the action that was not passed.</param>
        /// <param name="isCorrect">Indicates whether the action was passed correctly or not.</param>
        /// <param name="description">A description of the action that was not passed for the Log.</param>
        public event Action<Data.E_NameActionInteractable, bool, string> OnActionNotPassed;
        public void ActionFailed(Data.E_NameActionInteractable id, bool isCorrect = false, string description = "")
        {
            OnActionNotPassed?.Invoke(id, isCorrect, description);
        }

        /// <summary>
        /// Event triggered when an action has already been passed.
        /// </summary>
        /// <remarks>
        /// This event is used to notify when an action has already been completed, either correctly or incorrectly.
        /// </remarks>
        /// <param name="id">The ID of the action that was already passed.</param>
        /// <param name="isCorrect">Indicates whether the action was passed correctly or not.</param>
        /// <param name="description">A description of the action that was already passed for the Log.</param>
        public event Action<Data.E_NameActionInteractable, bool, string> OnActionAlreadyPassed;
        public void ActionAlreadyPassed(Data.E_NameActionInteractable id, bool isCorrect = false, string description = "")
        {
            OnActionAlreadyPassed?.Invoke(id, isCorrect, description);
        }
    }
}