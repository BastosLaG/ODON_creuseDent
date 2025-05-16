using System;
using UnityEngine;

public class Manager_outline : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private HighlitableItem tablet;
    [SerializeField] private HighlitableItem notebook;
    [SerializeField] private HighlitableItem pinceBrewer;
    [SerializeField] private HighlitableItem pinceAinsworth;
    [SerializeField] private HighlitableItem crampon;
    [SerializeField] private HighlitableItem damSupport;

    public enum ItemType
    {
        Tablet,
        Notebook,
        PinceBrewer,
        PinceAinsworth,
        Crampon,
        DamSupport
    }

    void Start()
    {
        UpdateOutline(tablet);
    }

    public void UpdateOutline(HighlitableItem highlitableItem)
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

    public GameObject GetItemGameObject(ItemType type)
    {
        return type switch
        {
            ItemType.Tablet => tablet.gameObject,
            ItemType.Notebook => notebook.gameObject,
            ItemType.PinceBrewer => pinceBrewer.gameObject,
            ItemType.PinceAinsworth => pinceAinsworth.gameObject,
            ItemType.Crampon => crampon.gameObject,
            ItemType.DamSupport => damSupport.gameObject,
            _ => null
        };
    }

}