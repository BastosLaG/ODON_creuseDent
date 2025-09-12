using UnityEngine;

namespace ODON.Data
{
    /// <summary>
    /// Serializable struct representing a tooth in the ODON system.
    /// Stores the quadrant (state), tooth GameObject, index, and provides a unique Id.
    /// </summary>
    [System.Serializable]
    public struct Struct_Teeth
    {
        /// <summary>
        /// The quadrant or state of the tooth.
        /// </summary>
        public StateTeeth state;

        /// <summary>
        /// Reference to the tooth GameObject.
        /// </summary>
        public GameObject tooth;

        /// <summary>
        /// The index of the tooth within the quadrant.
        /// </summary>
        public int index;

        /// <summary>
        /// Gets or sets the unique Id for the tooth, combining state and index.
        /// </summary>
        public int Id
        {
            get { return (int)state * 10 + index; }
            set
            {
                state = (StateTeeth)(value / 10);
                index = value % 10;
            }
        }
    }
}