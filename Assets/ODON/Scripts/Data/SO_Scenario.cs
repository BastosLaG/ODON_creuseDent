using UnityEngine;
using System.Collections.Generic;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "ODON/Scenario", order = 1)]
    public class SO_Scenario : ScriptableObject
    {
        [SerializeField] private List<SO_Step> key = new List<SO_Step>();
        [SerializeField] private List<E_NameActionInteractable> values = new List<E_NameActionInteractable>();
    }
}