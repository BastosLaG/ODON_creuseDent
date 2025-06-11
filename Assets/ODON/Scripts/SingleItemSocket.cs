using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ODON.Scripts
{
    /// <summary>
    /// Represents a single item socket in the game.
    /// </summary>
    /// <remarks>
    /// This class is used to manage the behavior of a single item socket.
    /// </remarks>
    public class SingleItemSocket : XRSocketInteractor
    {
        [SerializeField] private GameObject allowedItemName; // Or use a tag or scriptable ID

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return base.CanSelect(interactable) &&
                interactable.transform.name == allowedItemName.name;
        }
    }
}
