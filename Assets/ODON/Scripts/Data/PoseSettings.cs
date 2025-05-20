using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace ODON.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PoseSettings", menuName = "ODON/PoseSettings", order = 1)]
    public class PoseSettings : ScriptableObject
    {
        [SerializeField] private HighlightableItem[] highlightableItems;
        [SerializeField] private int currentIndex = 0;

        #region Public Methods

        void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            currentIndex = 0;
            foreach (Data.HighlightableItem item in highlightableItems)
            {
                item.isHighlightable = false;
                EnableCurrentOutline(item);
            }
            EnableCurrentOutline();
        }

        public void SwitchOutlineIncremente()
        {
            if (currentIndex < highlightableItems.Length - 1)
            {
                DisableCurrentOutline();
                currentIndex++;
                EnableCurrentOutline();
            }
        }

        public void SwitchOutlineDecremente()
        {
            if (currentIndex > 0)
            {
                DisableCurrentOutline();
                currentIndex--;
                EnableCurrentOutline();
            }
        }

        #endregion
        #region Validation

        public bool IsCorrectItem(GameObject clickedObject)
        {
            return highlightableItems[currentIndex]?.gameObject.name == clickedObject.name;
        }

        public bool TryValidateCurrentItem(GameObject clickedObject)
        {
            Debug.Log($"Clicked object: {clickedObject.name}");
            Debug.Log($"Current item: {highlightableItems[currentIndex]?.gameObject.name}");
            if (IsCorrectItem(clickedObject))
            {
                Debug.Log("Correct item clicked.");
                return true;
            }
            else
            {
                Debug.LogWarning("Incorrect item clicked.");
                return false;
            }
        }

        #endregion
        #region Private Methods

        public void DisableCurrentOutline()
        {
            highlightableItems[currentIndex].gameObject.TryGetComponent<Outline>(out var currentOutline);
            currentOutline.enabled = false;
        }

        public void EnableCurrentOutline()
        {
            highlightableItems[currentIndex].gameObject.TryGetComponent<Outline>(out var outline);
            outline.enabled = true;
        }

        public void EnableCurrentOutline(HighlightableItem highlightableItem)
        {
            if (highlightableItem?.gameObject == null)
            {
                Debug.LogError("HighlitableItem or its GameObject is null.");
                return;
            }
            if (!highlightableItem.gameObject.TryGetComponent<Outline>(out var outline))
            {
                Debug.Log("Adding Outline component to the GameObject.");
                outline = highlightableItem.gameObject.AddComponent<Outline>();
            }

            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = false;
        }

        #endregion

    }
}