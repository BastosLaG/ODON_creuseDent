using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private Data.Struct_Teeth goodTeethToDig;

        [SerializeField] private List<InteractableObject.PreviewDigDam> pDDs;

        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;
        [SerializeField] private UpdateShaderDam shaderDam;

        [Header("Pliers Settings")]
        [SerializeField] private GameObject pliers;

        // [Header("Sender")]
        // [SerializeField] private UniversalSenderActionToEventManager uSATEManager;

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

            // if (uSATEManager == null)
            // {
            //     Debug.LogError("UniversalSenderActionToEventManager is not assigned in HighlighteManager.");
            // }
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

            InitializePreview();
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        public void SetTeeth()
        {
            foreach (InteractableObject.PreviewDigDam pDD in pDDs)
            {
                if (pDD.MR.enabled == true && pDD.transform.name == goodTeethToDig.tooth.name)
                {
                    if (goodTeethToDig.tooth.name == pDD.transform.name)
                    {
                        SetTeeth(pDD.transform.name);
                        // uSATEManager.TryValidateCurrentItem();
                        return;
                    }
                }
                else if (pDD.MR.enabled == true && pDD.transform.name != goodTeethToDig.tooth.name)
                {
                    // uSATEManager.TryValidateCurrentItem(false);
                }
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

        private void InitializePreview()
        {
            pDDs = new List<InteractableObject.PreviewDigDam>(FindObjectsByType<InteractableObject.PreviewDigDam>(FindObjectsSortMode.None));
        }

        private void SetTeeth(string name)
        {
            if (name == "Null")
            {
                return;
            }

            Data.Struct_Teeth teeth = GetStateTeeth(name);
            if (teeth.index != 0)
            {
                shaderDam.TeethToManage = int.Parse(name);
                shaderDam.SwitchActiveHole(true);
            }
            else
            {
                Debug.LogWarning($"No teeth found for name: {name}");
            }
        }

        private Data.Struct_Teeth GetStateTeeth(string number)
        {
            int index = int.Parse(number);
            shaderDam.TeethToManage = index;
            foreach (Data.Struct_Teeth item in teethStructList)
            {
                if (item.index == index % 8 &&
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