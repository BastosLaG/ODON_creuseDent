using UnityEngine;
using System;
using System.Collections.Generic;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "ODON/Scenario", order = 1)]
    public class SO_Scenario : ScriptableObject
    {
        [SerializeField] private List<SO_Step> key = new List<SO_Step>();
        [SerializeField] private List<int> values = new List<int>();
    }
}