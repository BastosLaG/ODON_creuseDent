using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SetObjectGrabable : MonoBehaviour
{
    [SerializeField] private int _actionId = -1;
    [SerializeField] private InteractionLayerMask interactLayers = 2;
    [SerializeField] private bool _dynamicAttach = true, _itemSelected, _itemKinematic, _constrainRBody, _multipleGrab;

    void Start()
    {
        Rigidbody rb = null;
        if (transform.GetComponent<Rigidbody>() != null)
            rb = transform.GetComponent<Rigidbody>();
        else
            rb = transform.AddComponent<Rigidbody>();
        rb.isKinematic = _itemKinematic;
        rb.constraints = _constrainRBody ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        XRGrabInteractable grabScript = transform.AddComponent<XRGrabInteractable>();
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
    }
    public bool ItemIsSelected() { return _itemSelected; }
}
