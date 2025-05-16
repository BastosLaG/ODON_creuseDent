using System;
using Unity.VisualScripting;
using UnityEngine;


namespace ODON
{
    public class Manager_outline : MonoBehaviour
    {

        private static Manager_outline instance = null;
        public static Manager_outline Instance => instance;
        [SerializeField] private HighlitableItem[] highlitableItems;
        public HighlitableItem[] HighlitableItems => highlitableItems;

        public enum ItemType
        {
            Tablet,
            Notebook,
            PinceBrewer,
            PinceAinsworth,
            Crampon,
            DamSupport
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                instance = this;
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public void UpdateOutline(ItemType item)
        {
            switch (item)
            {
                case ItemType.Tablet:
                    EnableOutline(highlitableItems[0]);
                    break;
                case ItemType.Notebook:
                    EnableOutline(highlitableItems[1]);
                    break;
                case ItemType.PinceBrewer:
                    EnableOutline(highlitableItems[2]);
                    break;
                case ItemType.PinceAinsworth:
                    EnableOutline(highlitableItems[3]);
                    break;
                case ItemType.Crampon:
                    EnableOutline(highlitableItems[4]);
                    break;
                case ItemType.DamSupport:
                    EnableOutline(highlitableItems[5]);
                    break;
                default:
                    Debug.LogError("Invalid item type.");
                    break;
            }

        }

        private void EnableOutline(HighlitableItem highlitableItem)
        {
            if (highlitableItem?.gameObject == null)
            {
                Debug.LogError("HighlitableItem or its GameObject is null.");
                return;
            }
            if (!highlitableItem.isHighlitable)
            {
                return;
            }
            if (!highlitableItem.gameObject.TryGetComponent<Outline>(out var outline))
            {
                Debug.LogError("Outline component not found on the object.");
                return;
            }
            else
            {
                highlitableItem.isHighlitable = true;
                outline.enabled = true;
            }
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
        }

        public void DisableOutline(HighlitableItem highlitableItem)
        {
            if (highlitableItem.gameObject.TryGetComponent<Outline>(out var outline))
            {
                outline.enabled = false;
            }
        }

        internal void UpdateOutline(int etapeNum)
        {
            throw new NotImplementedException();
        }
    }
}