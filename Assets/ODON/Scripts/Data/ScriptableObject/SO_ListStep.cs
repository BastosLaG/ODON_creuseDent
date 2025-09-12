using UnityEngine;
using System.Collections.Generic;
using System;

namespace ODON.Data
{
    /// <summary>
    /// ScriptableObject representing a list of steps for a scenario in the ODON system.
    /// Stores and manages a collection of SO_Step objects.
    /// </summary>
    [CreateAssetMenu(fileName = "ListStep", menuName = "ODON/ListStep", order = 1)]
    public class SO_ListStep : ScriptableObject
    {
        /// <summary>
        /// The list of SO_Step objects representing scenario steps.
        /// </summary>
        [SerializeField] private List<SO_Step> list = new ();

        /// <summary>
        /// Gets or sets the list of SO_Step objects.
        /// </summary>
        public List<SO_Step> List {
            get => list;
            set { list = value; }
        }
    }
}