using System;
using UnityEngine;


namespace ODON.GameManager
{
    public class EventManager : MonoBehaviour
    {
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

        public event Action<int> OnActionCorrectlyPassed;
        public event Action<int> OnActionWronglyPassed;

        public void ActionCorrectlyPassed(int id)
        {
            if (OnActionCorrectlyPassed == null)
            {
                Debug.LogWarning("No listeners for OnActionCorrectlyPassed event.");
                return;
            }
            OnActionCorrectlyPassed?.Invoke(id);
        }

        public void ActionWronglyPassed(int actionId)
        {
            if (OnActionWronglyPassed == null)
            {
                Debug.LogWarning("No listeners for OnActionWronglyPassed event.");
                return;
            }
            OnActionWronglyPassed?.Invoke(actionId); 
        }
    }

}