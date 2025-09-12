using System;
using System.Collections.Generic;
using ODON.UsateManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace ODON.GameManager
{
    /// <summary>
    /// Manages highlighting and visibility of teeth for the digue (dam) system in the ODON application.
    /// Handles initialization of teeth, digue settings, pliers interactions, and events for hiding teeth.
    /// </summary>
    [Serializable]
    public class HighlightsTeethManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of HighlightsTeethManager.
        /// </summary>
        private static HighlightsTeethManager instance;

        /// <summary>
        /// Gets the singleton instance of HighlightsTeethManager.
        /// </summary>
        public static HighlightsTeethManager Instance => instance;

        #region Properties

        /// <summary>
        /// Array of Struct_Teeth representing all teeth in the scene.
        /// </summary>
        [Header("Teeth Settings")]
        [SerializeField] private Data.Struct_Teeth[] teethStructList;

        /// <summary>
        /// The tooth that should be dug (treated) according to patient data.
        /// </summary>
        [SerializeField] private Data.Struct_Teeth goodTeethToDig;

        /// <summary>
        /// Gets the tooth that should be dug (treated).
        /// </summary>
        public Data.Struct_Teeth GoodTeethToDig => goodTeethToDig;

        /// <summary>
        /// List of preview dig dam objects.
        /// </summary>
        [SerializeField, Obsolete("PreviewDigDam is deprecated, use the new interaction system.")] private List<InteractableObject.PreviewDigDam> pDDs;

        /// <summary>
        /// Reference to the digue (dam) GameObject.
        /// </summary>
        [Header("Digue Settings")]
        [SerializeField] private GameObject digue;

        /// <summary>
        /// Reference to the shader dam updater.
        /// </summary>
        [SerializeField] private UpdateShaderDam shaderDam;

        /// <summary>
        /// Reference to the pliers interaction manager.
        /// </summary>
        [Header("Pliers Settings")]
        [SerializeField] private USATEInteractWithPliers pliers;

        /// <summary>
        /// Event invoked when the dam is dug, used to hide or show teeth.
        /// </summary>
        public UnityEvent<bool> OnDigDam;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
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

        /// <summary>
        /// Initializes teeth data and sets the target tooth for pliers.
        /// </summary>
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

        /// <summary>
        /// Registers the HideAllTeeth listener to the OnDigDam event.
        /// </summary>
        void OnEnable()
        {
            OnDigDam.AddListener(HideAllTeeth);
        }

        /// <summary>
        /// Unregisters the HideAllTeeth listener from the OnDigDam event.
        /// </summary>
        void OnDisable()
        {
            OnDigDam.RemoveListener(HideAllTeeth);
        }

        /// <summary>
        /// Hides or shows all teeth based on the argument.
        /// </summary>
        /// <param name="arg0">If true, hides all teeth; if false, shows all teeth.</param>
        public void HideAllTeeth(bool arg0)
        {
            foreach (Data.Struct_Teeth teeth in teethStructList)
            {
                // Non arg0 pour respecter le nom de la fonction et suite logique des autres fonctions
                teeth.tooth.SetActive(!arg0);
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Private Methods

        /// <summary>
        /// Initializes the teethStructList array with Struct_Teeth for each child transform.
        /// </summary>
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