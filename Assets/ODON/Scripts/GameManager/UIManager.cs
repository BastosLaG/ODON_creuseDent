using UnityEngine;

namespace ODON.GameManager
{
    
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [SerializeField] private UI.UIClipBoard uIClipBoard;
        public UI.UIClipBoard UIClipBoard
        {
            get => uIClipBoard;
            set => uIClipBoard = value;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InitClipBoard()
        {
            UIClipBoard = FindFirstObjectByType<UI.UIClipBoard>();
            uIClipBoard.UpdateUI(GameHandler.Instance.PatientData);
        }
    }
}