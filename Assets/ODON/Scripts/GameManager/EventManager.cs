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

        public event Action<int, bool, string> OnActionPassed;

        public void ActionCorrectlyPassed(int id, bool isCorrect = false, string description = "")
        {
            if (OnActionPassed == null)
            {
                Debug.LogWarning("No listeners for OnActionCorrectlyPassed event.");
                return;
            }
            OnActionPassed?.Invoke(id, isCorrect, description);
        }
    }

}