using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SetObjectGrabable : MonoBehaviour
{
    [SerializeField] private int _actionId = -1;
    [SerializeField] private InteractionLayerMask interactLayers = 2;
    [SerializeField] private bool _dynamicAttach = true, _itemSelected, _itemKinematic, _constrainRBody, _multipleGrab;

    private Vector3 grabPoint;
    private Transform handTransform;

    void Start()
    {
        Rigidbody rb = null;
        if (transform.GetComponent<Rigidbody>() != null)
            rb = gameObject.GetComponent<Rigidbody>();
        else
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = _itemKinematic;
        rb.constraints = _constrainRBody ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;

        XRGrabInteractable grabScript = gameObject.AddComponent<XRGrabInteractable>();
        grabScript.interactionLayers = interactLayers;
        grabScript.selectMode = _multipleGrab ? InteractableSelectMode.Multiple : InteractableSelectMode.Single;
        grabScript.useDynamicAttach = _dynamicAttach;
        grabScript.activated.AddListener(delegate { SetAction(_actionId); });
        grabScript.selectEntered.AddListener(delegate { SetSelected(true); });
        grabScript.selectExited.AddListener(delegate { SetSelected(false); });
    }

    private void SetAction(int id)
    {
        switch (id)
        {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }
    }

    private void SetSelected(bool isSelected)
    {
        _itemSelected = isSelected;

        if (isSelected)
        {
            handTransform = GetInteractorTransform();
            grabPoint = handTransform.position;
            ProvideHapticFeedback();
        }
        else
        {
            handTransform = null;
        }
    }

    private Transform GetInteractorTransform()
    {
        if (TryGetComponent(out XRGrabInteractable grabScript))
        {
            var interactor = grabScript.firstInteractorSelecting;
            if (interactor != null)
                return interactor.transform;
        }
        return null;
    }

    private void ApplyForceAtGrabPoint()
    {
        if (!_itemSelected || handTransform == null) return;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = (handTransform.position - grabPoint).normalized;
            float forceMagnitude = 10f;
            rb.AddForceAtPosition(forceDirection * forceMagnitude, grabPoint);
        }
    }

    private void ProvideHapticFeedback()
    {
        if (TryGetComponent(out XRGrabInteractable grabScript))
        {
            foreach (var interactor in grabScript.interactorsSelecting)
            {
                if (interactor is XRBaseInputInteractor controllerInteractor)
                {
                    controllerInteractor.SendHapticImpulse(0.5f, 0.2f); // Intensité et durée
                }
            }
        }
    }

    void FixedUpdate()
    {
        ApplyForceAtGrabPoint();
    }

    internal bool ItemIsSelected() { return _itemSelected; }
}
