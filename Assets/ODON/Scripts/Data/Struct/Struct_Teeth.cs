using UnityEngine;

namespace ODON.Data
{
    [System.Serializable]
    public struct Struct_Teeth
    {
        public StateTeeth state;
        public GameObject tooth;
        public int index;

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