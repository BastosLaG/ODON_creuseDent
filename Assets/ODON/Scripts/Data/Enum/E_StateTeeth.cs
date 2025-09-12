using System.Collections.Generic;
using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// Enumeration representing the possible quadrants of teeth in the ODON system.
    /// </summary>
    [System.Serializable]
    public enum StateTeeth
    {
        /// <summary>
        /// Upper right quadrant.
        /// </summary>
        UPPERRIGHT = 1,
        /// <summary>
        /// Upper left quadrant.
        /// </summary>
        UPPERLEFT = 2,
        /// <summary>
        /// Lower left quadrant.
        /// </summary>
        LOWERLEFT = 3,
        /// <summary>
        /// Lower right quadrant.
        /// </summary>
        LOWERRIGHT = 4
    }
}