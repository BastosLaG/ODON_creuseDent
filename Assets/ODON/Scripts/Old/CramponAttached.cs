using System;
using UnityEngine;

[Obsolete("CramponAttached is deprecated, use the new interaction system.")]
public class CramponAttached : MonoBehaviour
{
    [Header("This attachment point")]
    public Transform attachPoint1;
    public Transform attachPoint2;
    [Header("Target attachment point")]
    public Transform targetAttachPoint1;
    public Transform targetAttachPoint2;
    public Transform targetTransform;

    [Header("Booléen")]
    public bool isCanAttach = false;
    private bool isAttached = false;

    private void Update()
    {
        if (isAttached)
        {
            transform.position = targetAttachPoint1.position;
            // transform.rotation = targetAttachPoint1.rotation;

            attachPoint2.position = targetAttachPoint2.position;
            // attachPoint2.rotation = targetAttachPoint2.rotation;

            targetTransform.position = targetTransform.position;
            // targetTransform.rotation = targetTransform.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crampon"))
        {
            isCanAttach = true;
            Debug.Log($"L'objet {other.name} peut maintenant être attaché.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Crampon"))
        {
            isCanAttach = false;
            Debug.Log($"L'objet {other.name} ne peut plus être attaché.");
        }
    }

    public void AttachObject()
    {
        if (isAttached)
        {
            isAttached = false;
            Debug.LogWarning("Détachement du crampon.");
            return;
        }
        if (!isCanAttach)
        {
            Debug.LogWarning("Attachement non autorisé.");
            return;
        }

        if (attachPoint1 != null && attachPoint2 != null && targetAttachPoint1 != null && targetAttachPoint2 != null)
        {
            isAttached = true;
            Debug.Log($"Crampon a été attaché.");
        }
        else
        {
            Debug.LogError("Points d'attache ou objet cible manquants.");
        }
    }
}