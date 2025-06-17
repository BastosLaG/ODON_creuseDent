using ODON;
using UnityEngine;

namespace ODON.GameManager
{
    public class HighlightsManager : MonoBehaviour
    {
        private static HighlightsManager instance;
        public static HighlightsManager Instance => instance;

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

        public void InitHighLight()
        {
            // GetSequenceForTechnique(GameHandler.Instance.TechniqueId);

            // foreach (item in )
            // {
            //     if (item.gameObject == null)
            //     {
            //         Debug.LogError("GameObject is null for item: " + item);
            //         continue;
            //     }
            //     Manager_outline.AddOutline(item.gameObject);
            // }
            // GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].EnableOutline();
        }

        public void SwitchActiveItem(int amount)
        {
            // if (GameHandler.Instance.CurrentHighlightableIndex + amount < 0 || GameHandler.Instance.CurrentHighlightableIndex + amount >= GameHandler.Instance.HighlightableItems.Length)
            // {
            //     Debug.LogError("Index out of range.");
            //     return;
            // }
            // GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].DisableOutline();
            // GameHandler.Instance.CurrentHighlightableIndex += amount;
            // GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].EnableOutline();
        }

        public bool TryValidateCurrentItem(Data.SO_Step step)
        {
            if (step == null)
            {
                Debug.LogError("Step is null.");
                return false;
            }

            bool isValid = true;

            if (isValid)
            {
                EventManager.Instance.ActionCorrectlyPassed(step.Id, true, step.Description);
                return true;
            }
            else
            {
                EventManager.Instance.ActionFailed(step.Id, false, step.Description);
                return false;
            }
        }
    }
}

