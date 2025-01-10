using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MoveColliderToHand : MonoBehaviour
{
    [SerializeField] private BoxCollider targetCollider; // Collider to move
    private Vector3 originalPosition; // Store the initial position of the collider
    private bool isSelected = false;

    void Start()
    {
        if (targetCollider == null)
        {
            Debug.LogError("Target Collider is not assigned.");
            return;
        }

        // Store the original position of the collider
        originalPosition = targetCollider.transform.localPosition;

        // Add interaction listeners
        var grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isSelected = true;

        // Move the collider to the hand's position
        Transform handTransform = GetInteractorTransform(args.interactorObject);
        if (handTransform != null && targetCollider != null)
        {
            targetCollider.transform.position = handTransform.position;
        }
    }

    private Transform GetInteractorTransform(IXRSelectInteractor interactorObject)
    {
        throw new NotImplementedException();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isSelected = false;

        // Reset the collider to its original position
        if (targetCollider != null)
        {
            targetCollider.transform.localPosition = originalPosition;
        }
    }

    void Update()
    {
        if (isSelected && targetCollider != null)
        {
            // Continuously update the collider's position to match the hand's position
            Transform handTransform = GetInteractorTransform();
            if (handTransform != null)
            {
                targetCollider.transform.position = handTransform.position;
            }
        }
    }

    private Transform GetInteractorTransform(UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor = null)
    {
        // Get the interactor's transform (hand position)
        if (interactor != null)
        {
            return interactor.transform;
        }

        // Fallback to the first interactor selecting the object
        if (TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabScript))
        {
            var firstInteractor = grabScript.firstInteractorSelecting;
            if (firstInteractor != null)
            {
                return firstInteractor.transform;
            }
        }
        return null;
    }
}
