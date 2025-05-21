using ODON.Data;
using UnityEngine;

namespace ODON
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler instance;
        public static GameHandler Instance => instance;

        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private GameObject tablet;
        [SerializeField] private GameObject clipBoard;
        [SerializeField] private GameObject pinceBrewer;
        [SerializeField] private GameObject pinceAinsworth;
        [SerializeField] private GameObject crampon;
        [SerializeField] private GameObject cadreEnU;
        [SerializeField] private GameObject dents;
        [SerializeField] private GameObject SupportDigue;

        [Header("Highlightable Items")]
        [Tooltip("List of items to be highlighted.")]
        [SerializeField] private HighlightableItem[] highlightableItems;
        [SerializeField] private int currentIndex = 0;
        [SerializeField] private int techniqueId = 0;

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

        private void Start()
        {
            highlightableItems = GetSequenceForTechnique(techniqueId);

            foreach (HighlightableItem item in highlightableItems)
            {
                Manager_outline.AddOutline(item.gameObject);
            }
            highlightableItems[currentIndex].EnableOutline();
        }

        public void SwitchActiveItem(int amount)
        {
            if (amount < 0 || amount >= highlightableItems.Length)
            {
                Debug.LogError("Index out of range.");
                return;
            }
            highlightableItems[currentIndex].DisableOutline();
            currentIndex += amount;
            highlightableItems[currentIndex].EnableOutline();
        }

        public bool TryValidateCurrentItem(GameObject clickedObject)
        {
            Debug.Log($"Clicked object: {clickedObject}");
            Debug.Log($"Current item: {highlightableItems[currentIndex]?.gameObject}");

            if (highlightableItems[currentIndex].gameObject == clickedObject)
            {
                Debug.Log("Correct item clicked.");
                return true;
            }
            else
            {
                Debug.Log("Incorrect item clicked.");
                return false;
            }
        }

        private HighlightableItem[] GetSequenceForTechnique(int techniqueId)
        {
            switch (techniqueId)
            {
                case 0: // Pose classique
                    return new HighlightableItem[]
                    {
                        new() { gameObject = tablet, isHighlightable = false },
                        new() { gameObject = clipBoard, isHighlightable = false },
                        new() { gameObject = pinceBrewer, isHighlightable = false }
                    };

                case 1: // Pose parachute
                    return new HighlightableItem[]
                    {
                        new() { gameObject = tablet, isHighlightable = false },
                        new() { gameObject = clipBoard, isHighlightable = false }
                    };

                default:
                    Debug.LogWarning("Technique ID inconnue.");
                    return new HighlightableItem[0];
            }
        }
    }
}