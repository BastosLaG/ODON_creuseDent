using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SetObjectGrabable : MonoBehaviour
{
    [SerializeField] private int _actionId = -1;
    [SerializeField] private InteractionLayerMask interactLayers = 2;
    [SerializeField] private bool _dynamicAttach = true;

    void Start()
    {
        XRGrabInteractable grabScript = transform.AddComponent<XRGrabInteractable>();
        grabScript.interactionLayers = interactLayers;
        grabScript.useDynamicAttach = _dynamicAttach;
        grabScript.activated.AddListener(delegate { SetAction(_actionId); });
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
}
