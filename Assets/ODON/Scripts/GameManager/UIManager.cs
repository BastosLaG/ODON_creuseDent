using UnityEngine;
using TMPro;

namespace ODON.GameManager
{
    /// <summary>
    /// Manages the UI elements for the ODON application.
    /// Handles debug log display and provides singleton access to UIManager.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of UIManager.
        /// </summary>
        private static UIManager instance;

        /// <summary>
        /// Gets the singleton instance of UIManager.
        /// </summary>
        public static UIManager Instance => instance;

        /// <summary>
        /// Reference to the TextMeshProUGUI component used for debug logging.
        /// </summary>
        [SerializeField] private TextMeshProUGUI UGUIDebugLog;

        /// <summary>
        /// Gets or sets the debug log text displayed in the UI.
        /// </summary>
        public string TextUIDebugLog
        {
            get => UGUIDebugLog.text;
            set => UGUIDebugLog.text = value;
        }

        /// <summary>
        /// Initializes the singleton instance on Awake.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// Sets the debug log text in the UI.
        /// </summary>
        /// <param name="text">The text to display in the debug log UI.</param>
        public void DebugLogTextUI(string text)
        {
            TextUIDebugLog = text;
        }
    }
}
