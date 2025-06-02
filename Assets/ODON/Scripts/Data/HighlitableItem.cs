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
            Outline outline = GetOutline();
            outline.enabled = true;
            isHighlightable = true;
        }
        public void DisableOutline()
        {
            Outline outline = GetOutline();
            outline.enabled = false;
            isHighlightable = false;
        }

        private Outline GetOutline()
        {
            gameObject.TryGetComponent<Outline>(out var outline);
            if (outline == null)
            {
                outline = gameObject.AddComponent<Outline>();
            }
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;

            return outline;
        }
    }
}