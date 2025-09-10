using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace ODON.GameManager
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [SerializeField] private TextMeshProUGUI DebugLogUGUI;
        public string DebugLogTextUI
        {
            get => DebugLogUGUI.text;
            set => DebugLogUGUI.text = value;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void OnTriggerStarted(InputAction.CallbackContext ctx)
        {
            // Debug.Log("Trigger pressed!");
            DebugLogTextUI = "Trigger pressed!";
        }
    }
}
