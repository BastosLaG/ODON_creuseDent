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
            GameHandler.Instance.HighlightableItems = GetSequenceForTechnique(GameHandler.Instance.TechniqueId);

            foreach (Data.HighlightableItem item in GameHandler.Instance.HighlightableItems)
            {
                if (item.gameObject == null)
                {
                    Debug.LogError("GameObject is null for item: " + item);
                    continue;
                }
                Manager_outline.AddOutline(item.gameObject);
            }
            GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].EnableOutline();
        }

        public void SwitchActiveItem(int amount)
        {
            if (GameHandler.Instance.CurrentHighlightableIndex + amount < 0 || GameHandler.Instance.CurrentHighlightableIndex + amount >= GameHandler.Instance.HighlightableItems.Length)
            {
                Debug.LogError("Index out of range.");
                return;
            }
            GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].DisableOutline();
            GameHandler.Instance.CurrentHighlightableIndex += amount;
            GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].EnableOutline();
        }
        

        public bool TryValidateCurrentItem(GameObject clickedObject)
        {
            if (GameHandler.Instance.CurrentHighlightableIndex >= GameHandler.Instance.HighlightableItems.Length)
            {
                Debug.LogError("Index out of range. Current index: " + GameHandler.Instance.CurrentHighlightableIndex + ", Length: " + GameHandler.Instance.HighlightableItems.Length);
                return false;
            }
            
            if (GameHandler.Instance.HighlightableItems[GameHandler.Instance.CurrentHighlightableIndex].gameObject == clickedObject)
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



        private Data.HighlightableItem[] GetSequenceForTechnique(int techniqueId)
        {
            switch (techniqueId)
            {
                case 0: // Pose classic
                    return new Data.HighlightableItem[]
                    {
                        new() { gameObject = GameHandler.Instance.Tablet, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.Door, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.ClipBoard, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.SecurityEquipment, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.SupportDigue, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.PliersAinsworth, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.PliersBrewer, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.Crampon, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.Digue, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.LowerDenture, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.CadreEnU, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.DentalFloss, isHighlightable = false }
                    };

                case 1: // Pose parachute
                    return new Data.HighlightableItem[]
                    {
                        new() { gameObject = GameHandler.Instance.Tablet, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.Door, isHighlightable = false },
                        new() { gameObject = GameHandler.Instance.ClipBoard, isHighlightable = false }
                    };

                default:
                    Debug.LogWarning("Technique ID unknown.");
                    return new Data.HighlightableItem[0];
            }
        }
    }
}
