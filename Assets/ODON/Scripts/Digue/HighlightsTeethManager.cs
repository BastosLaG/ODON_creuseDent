using System;
using UnityEngine;

namespace ODON.GameManager
{
    [Serializable]
    public class HighlightsTeethManager : MonoBehaviour
    {
        private static HighlightsTeethManager instance;
        public static HighlightsTeethManager Instance => instance;

        #region Properties
        [Header("Teeth Settings")]
        [SerializeField] private Data.Struct_Teeth[] teethStructList;
        [SerializeField] private Data.Struct_Teeth currentTheeth;

        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;
        [SerializeField] private UpdateShaderDam shaderDam;

        [Header("Pliers Settings")]
        [SerializeField] private GameObject pliers;

        public event Action<string> OnTriggerEnterEvent;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            InitializeTeeth();

            OnTriggerEnterEvent += SetTeeth;
        }

        void OnDisable()
        {
            OnTriggerEnterEvent -= SetTeeth;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        public void SetTeeth(bool isPliers = false)
        {
            if (isPliers)
            {
                shaderDam.DamMaterial.SetInteger("_IsHoleActive", isPliers ? 1 : 0);     
            }
        }

        public string InvokeOnTriggerEnterEvent(string name)
        {
            OnTriggerEnterEvent?.Invoke(name);
            return name;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods
        private void SetTeeth(string name)
        {
            if (name == "Null")
            {
                SetTeeth(new Data.Struct_Teeth { index = 0, state = Data.StateTeeth.UPPERRIGHT });
                return;
            }

            Data.Struct_Teeth teeth = GetStateTeeth(name);
            if (teeth.index != 0)
            {
                SetTeeth(teeth);
            }
            else
            {
                Debug.LogWarning($"No teeth found for name: {name}");
            }
        }
        private void InitializeTeeth()
        {
            int childCount = transform.childCount;
            teethStructList = new Data.Struct_Teeth[childCount];

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Data.Struct_Teeth toothStruct = new()
                {
                    tooth = child.gameObject,
                    index = i % 8 + 1,
                    state = (i <= 7) ? Data.StateTeeth.UPPERRIGHT :
                            (i <= 15) ? Data.StateTeeth.UPPERLEFT :
                            (i <= 23) ? Data.StateTeeth.LOWERLEFT :
                                        Data.StateTeeth.LOWERRIGHT
                };

                teethStructList[i] = toothStruct;
            }
        }

        private void SetTeeth(Data.Struct_Teeth teeth)
        {
            shaderDam.DamMaterial.SetInteger("TeethIndex", ((int)teeth.state * 10) + teeth.index);
        }

        private Data.Struct_Teeth GetStateTeeth(string number)
        {
            int index = int.Parse(number);
            shaderDam.TeethToManage = index;
            Debug.Log($"GetStateTeeth called with index: {index}");
            foreach (Data.Struct_Teeth item in teethStructList)
            {
                if (item.index == index % 8 + 1 &&
                    item.state == (index <= 7 ? Data.StateTeeth.UPPERRIGHT :
                                   index <= 15 ? Data.StateTeeth.UPPERLEFT :
                                   index <= 23 ? Data.StateTeeth.LOWERLEFT :
                                                 Data.StateTeeth.LOWERRIGHT))
                {
                    return item;
                }
            }
            Debug.LogWarning($"No tooth found for index {index}");
            return default;
        }

        #endregion
    }
}