using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SetObjectGrabable : MonoBehaviour
{
    [SerializeField] private InteractionLayerMask interactLayers = 2;
    [SerializeField] private bool _dynamicAttach = true, _itemSelected, _itemKinematic, _constrainRBody, _multipleGrab;

    [SerializeField] private bool doEventOneTime = false;
    [SerializeField] private UnityEvent SelectEnter, SelectExit, ActionEnter, ActionExit;

    private Vector3 grabPoint;
    private Transform handTransform;
    private BoxCollider boxCollider;
    private XRGrabInteractable grabScript;


    void Start()
    {
        Rigidbody rb = null;
        boxCollider = null;
        if (transform.GetComponent<Rigidbody>() != null)
            rb = gameObject.GetComponent<Rigidbody>();
        else
            rb = gameObject.AddComponent<Rigidbody>();
        if (transform.GetComponent<BoxCollider>() != null)
            boxCollider = gameObject.GetComponent<BoxCollider>();
        rb.isKinematic = _itemKinematic;
        rb.constraints = _constrainRBody ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;

        grabScript = gameObject.AddComponent<XRGrabInteractable>();
        grabScript.interactionLayers = interactLayers;
        grabScript.selectMode = _multipleGrab ? InteractableSelectMode.Multiple : InteractableSelectMode.Single;
        grabScript.useDynamicAttach = _dynamicAttach;
        grabScript.activated.AddListener(OnActionEnter);
        grabScript.deactivated.AddListener(OnActionExit);
        grabScript.selectEntered.AddListener(OnSelectEnter);
        grabScript.selectExited.AddListener(OnSelectExit);
        grabScript.selectEntered.AddListener(delegate { SetSelected(true); });
        grabScript.selectExited.AddListener(delegate { SetSelected(false); });
    }

    private void OnSelectEnter(SelectEnterEventArgs args)
    {
        SelectEnter?.Invoke();
        if (doEventOneTime) grabScript.selectEntered.RemoveAllListeners();
        grabScript.selectEntered.AddListener(delegate { SetSelected(true); });
    }

    private void OnSelectExit(SelectExitEventArgs args)
    {
        SelectExit?.Invoke();
        if (doEventOneTime) grabScript.selectExited.RemoveAllListeners();
        grabScript.selectExited.AddListener(delegate { SetSelected(false); });
    }

    private void OnActionEnter(ActivateEventArgs args)
    {
        ActionEnter?.Invoke();
        if (doEventOneTime) grabScript.activated.RemoveAllListeners();
    }

    private void OnActionExit(DeactivateEventArgs args)
    {
        ActionExit?.Invoke();
        if (doEventOneTime) grabScript.deactivated.RemoveAllListeners();
    }

    private void SetSelected(bool isSelected)
    {
        _itemSelected = isSelected;

        if (isSelected)
        {
            handTransform = GetInteractorTransform();
            grabPoint = handTransform.position;
            ProvideHapticFeedback();

            // Désactiver le BoxCollider
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }
        else
        {
            handTransform = null;

            // Réactiver le BoxCollider
            if (boxCollider != null)
            {
                boxCollider.enabled = true;
            }
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
