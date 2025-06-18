using UnityEngine;

namespace ODON.Data
{

    [CreateAssetMenu(fileName = "Step", menuName = "ODON/Step", order = 1)]
    public class SO_Step : ScriptableObject
    {
        [SerializeField] private E_NameActionInteractable id = E_NameActionInteractable.None;
        [SerializeField] private string description;
        [SerializeField] private bool isActive = true;

        public E_NameActionInteractable Id => id;
        public string Description => description;

        public void ActionPassed()
        {
            GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(id, true, description);
        }
    }
}