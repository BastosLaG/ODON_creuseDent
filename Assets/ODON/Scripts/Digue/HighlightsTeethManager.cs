using System;
using ODON.Scripts.Digue;
using UnityEngine;

namespace ODON.GameManager.Digue
{
    [Serializable]
    public class HighlightsTeethManager : MonoBehaviour
    {
        #region Properties
        [Header("Teeth Settings")]
        [SerializeField] private Data.Struct_Teeth[] teethStructList;
        public int currentState;
        [SerializeField] private int maxState = 3;
        public int goodState = 0;

        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;
        [SerializeField] private UpdateShaderDam shaderDam;

        [Header("Pliers Settings")]
        [SerializeField] private GameObject pliers;
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods
        void Start()
        {
            currentState = 0;
            InitializeTeeth();
            InitializeDigue();
            CleanTeeth();
            SwitchState(0);
        }

        void Update()
        {
            foreach (var item in teethStructList)
            {
                if (item.tooth.activeSelf)
                {
                    goodState = (int)item.state;
                    break;
                }
            }
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        public void SwitchState(int increment) // TODO : change this method to select with hand the thooth we want to highlight
        {
            currentState = (currentState + increment + maxState) % maxState;

            CleanTeeth();

            switch (currentState)
            {
                case 0:
                    SetTeeth(Data.StateTeeth.LOWERRIGHT, 1);
                    break;
                case 1:
                    SetTeeth(Data.StateTeeth.LOWERRIGHT, 3);
                    break;
                case 2:
                    SetTeeth(Data.StateTeeth.LOWERRIGHT, 7);
                    break;
            }
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
                Data.Struct_Teeth toothStruct = new ()
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

        private void InitializeDigue()
        {
            if (digue == null)
            {
                Debug.LogError("Digue is not assigned.");
                return;
            }
            shaderDam = digue.GetComponent<UpdateShaderDam>();
            if (shaderDam == null)
            {
                Debug.LogError("UpdateShaderDam component is not found on the digue.");
                return;
            }

            shaderDam.TeethToManage = ((int)Data.StateTeeth.LOWERRIGHT*10) + 1; // Initialize with the good tooth to digue in change this by gamehandler state later
        }

        private void SetTeeth(Data.StateTeeth state, int indexList)
        {
            foreach (var item in teethStructList)
            {
                if (item.state == state && item.index == indexList)
                {
                    item.tooth.SetActive(true);
                    shaderDam.DamMaterial.SetInt("TeethIndex", ((int)state * 10) + item.index);
                    return;
                }
                else
                {
                    item.tooth.SetActive(false);
                }
            }
        }
        
        private void CleanTeeth()
        {
            for (int i = 0; i < teethStructList.Length; i++)
            {
                teethStructList[i].tooth.SetActive(false);
            }
        }

        #endregion
    }
}