using UnityEngine;

namespace ODON.Data
{

    [CreateAssetMenu(fileName = "Step", menuName = "ODON/Step", order = 1)]
    public class SO_Step : ScriptableObject
    {
        [SerializeField] private E_NameActionInteractable id = E_NameActionInteractable.None;
        [SerializeField] private string description;

        public E_NameActionInteractable Id => id;
        public string Description => description;

        public void ActionPassed()
        {
            GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(id, true, description);
        }

        public void ActionFailed()
        {
            GameManager.EventManager.Instance.Scenario.UpdateCurrentValueIndex(id, false, description);
        }
    }
}