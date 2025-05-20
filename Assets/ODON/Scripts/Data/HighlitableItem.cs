using UnityEngine;


namespace ODON.Data
{
    [System.Serializable]
    public class HighlightableItem
    {
        public GameObject gameObject;
        public bool isHighlightable = false;

        public void EnableOutline()
        {
            isHighlightable = true;
        }
        public void DisableOutline()
        {
            isHighlightable = false;
        }
    }
}