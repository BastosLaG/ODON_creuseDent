using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.Data
{
    [CreateAssetMenu(fileName = "ListStep", menuName = "ODON/ListStep", order = 1)]
    public class SO_ListStep : ScriptableObject
    {
        [SerializeField] private List<SO_Step> list = new List<SO_Step>();
        public List<SO_Step> List {
            get => list;
            set { list = value; }
        }
    }
}