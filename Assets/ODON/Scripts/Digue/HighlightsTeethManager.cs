using System;
using System.Collections.Generic;
using ODON.UsateManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private Data.Struct_Teeth goodTeethToDig;
        public Data.Struct_Teeth GoodTeethToDig => goodTeethToDig;

        [SerializeField] private List<InteractableObject.PreviewDigDam> pDDs;

        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;
        [SerializeField] private UpdateShaderDam shaderDam;

        [Header("Pliers Settings")]
        [SerializeField] private USATEInteractWithPliers pliers;

        public UnityEvent<bool> OnDigDam;


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

            foreach (Data.Struct_Teeth teeth in teethStructList)
            {
                if (teeth.tooth.name == GameHandler.Instance.PatientData.TreatedToothWithSection.ToString())
                {
                    goodTeethToDig = teeth;
                }
            }

            pliers.TargetObject = goodTeethToDig.tooth;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods

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
        #endregion
    }
}