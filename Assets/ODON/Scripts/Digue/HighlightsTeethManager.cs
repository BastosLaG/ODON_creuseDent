using System;
using System.Collections.Generic;
using ODON.Scripts.Digue;
using UnityEngine;
using UnityEngine.Events;

namespace ODON.GameManager.Digue
{
    [Serializable]
    public class HighlightsTeethManager : MonoBehaviour
    {
        [SerializeField] private TeethStruct[] teethStructList;
        public int currentState;
        [SerializeField] private int maxState = 3;

        public int goodState = 0;
        public UnityEvent m_IsGoodStateEvent;
        public UnityEvent m_IsNotGoodStateEvent;

        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;
        [SerializeField] private UpdateShaderDam shaderDam;

        [System.Serializable]
        public struct TeethStruct
        {
            public Data.StateTeeth state;
            public GameObject tooth;
            public int index;
        }

        void Start()
        {
            if (m_IsGoodStateEvent == null)
                m_IsGoodStateEvent = new UnityEvent();
            if (m_IsNotGoodStateEvent == null)
                m_IsNotGoodStateEvent = new UnityEvent();
            currentState = 0;
            InitializeTeeth();
            InitializeDigue();
            CleanTeeth();
            SwitchState(0);
        }

        private void InitializeTeeth()
        {
            int childCount = transform.childCount;
            teethStructList = new TeethStruct[childCount];

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                TeethStruct toothStruct = new TeethStruct
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

        public void SwitchState(int increment)
        {
            currentState = (currentState + increment + maxState) % maxState;

            CleanTeeth();

            if (currentState == goodState)
            {
                m_IsGoodStateEvent?.Invoke();
            }
            else
            {
                m_IsNotGoodStateEvent?.Invoke();
            }

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

        private void CleanTeeth()
        {
            for (int i = 0; i < teethStructList.Length; i++)
            {
                teethStructList[i].tooth.SetActive(false);
            }
        }
    }
}