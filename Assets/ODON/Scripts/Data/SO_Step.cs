using UnityEngine;

namespace ODON.Data
{
        
    [CreateAssetMenu(fileName = "Step", menuName = "ODON/Step", order = 1)]
    public class SO_Step : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string description;
        [SerializeField] private bool isCorrect;

        public int Id => id;
        public string Description => description;
        public bool IsCorrect {
            get => isCorrect;
            set => isCorrect = value;
        }

        public void ExecuteStep()
        {
            Debug.Log($"Step {id} execution is {isCorrect} : {description}");
            GameManager.EventManager.Instance.ActionCorrectlyPassed(id, isCorrect, description);
        }
    }

}