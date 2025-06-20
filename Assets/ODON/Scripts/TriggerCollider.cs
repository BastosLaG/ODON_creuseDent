using System;
using ODON.GameManager;
using UnityEngine;


public class TriggerCollider : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private LayerMask targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            targetObject.SetActive(true);
            HighlightsTeethManager.Instance?.InvokeOnTriggerEnterEvent(gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            targetObject.SetActive(false);
            HighlightsTeethManager.Instance?.InvokeOnTriggerEnterEvent("Null");
        }
    }
}
