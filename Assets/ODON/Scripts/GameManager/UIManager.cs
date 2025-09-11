using UnityEngine;
using TMPro;

namespace ODON.GameManager
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [SerializeField] private TextMeshProUGUI UGUIDebugLog;
        public string TextUIDebugLog
        {
            get => UGUIDebugLog.text;
            set => UGUIDebugLog.text = value;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void DebugLogTextUI(string text)
        {
            TextUIDebugLog = text;
        }
    }
}
