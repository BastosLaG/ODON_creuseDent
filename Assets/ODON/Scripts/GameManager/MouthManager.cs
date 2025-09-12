using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ODON.GameManager
{
    /// <summary>
    /// Manages the mouth state in the ODON application.
    /// Handles initialization and management of teeth and crampon objects, prefab swapping, and validation logic.
    /// </summary>
    [Obsolete("MouthManager is deprecated, use the new interaction system.")]
    public class MouthManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of MouthManager.
        /// </summary>
        [SerializeField] private static MouthManager instance;

        /// <summary>
        /// Gets the singleton instance of MouthManager.
        /// </summary>
        public static MouthManager Instance => instance;

        /// <summary>
        /// Dictionary mapping tooth keys to their corresponding GameObjects.
        /// </summary>
        [SerializeField] private Dictionary<int, GameObject> teethDictionary = new();

        /// <summary>
        /// Dictionary mapping tooth keys to their corresponding crampon GameObjects.
        /// </summary>
        [SerializeField] private Dictionary<int, GameObject> cramponDictionary = new();

        /// <summary>
        /// List of CramponPreview components for all crampons.
        /// </summary>
        [SerializeField] private List<CramponPreview> CramponList = new();

        /// <summary>
        /// Prefab for preview crampon objects.
        /// </summary>
        [SerializeField] private GameObject prefabPreviewCrampon;

        /// <summary>
        /// Prefab for crampon objects placed in the mouth.
        /// </summary>
        [SerializeField] private GameObject prefabCramponInMouth;

        // [SerializeField] private UniversalSenderActionToEventManager USATEManagerPutCrampon;

        /// <summary>
        /// Initializes the singleton instance on Awake.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initializes teeth and crampon dictionaries, instantiates preview crampons, and logs the treated tooth.
        /// </summary>
        private void Start()
        {
            foreach (Transform child in transform)
            {
                int key = GetTeeth(child.gameObject.name);
                teethDictionary.Add(key, child.gameObject);

                GameObject crampon = Instantiate(
                                    prefabPreviewCrampon,
                                    child.GetChild(0).position,
                                    child.GetChild(0).rotation,
                                    child.GetChild(0)
                                    );

                CramponPreview cP = crampon.GetComponent<CramponPreview>();
                cP.Init();
                CramponList.Add(cP);
                cramponDictionary.Add(key, crampon);
            }

            Debug.Log($"teeth needed is the number {GameHandler.Instance.PatientData.TreatedToothWithSection}");
        }

        /// <summary>
        /// Swaps preview crampon prefabs with in-mouth crampon prefabs for the treated tooth if validation is successful.
        /// </summary>
        public void SwapPrefabs()
        {
            int GoodKey = GameHandler.Instance.PatientData.TreatedToothWithSection;

            foreach (CramponPreview crampon in CramponList)
            {
                if (crampon.IsValid == true)
                {
                    string name = crampon.transform.parent.parent.gameObject.name;
                    int key = GetTeeth(name);
                    Debug.LogWarning($"Name of the teeth : {name}, key return {key} and Key we need {GoodKey}");
                    if (key == GoodKey)
                    {
                        cramponDictionary[key]  =   Instantiate
                                                    (
                                                        prefabCramponInMouth,
                                                        cramponDictionary[key].transform.position,
                                                        cramponDictionary[key].transform.rotation,
                                                        cramponDictionary[key].transform
                                                    );
                        
                        // USATEManagerPutCrampon.TryValidateCurrentItem();
                        return;
                    }
                    else
                    {
                        // USATEManagerPutCrampon.TryValidateCurrentItem(false);
                        return;
                    }   
                }
            }
        }

        /// <summary>
        /// Extracts the numeric tooth key from a GameObject name using regular expressions.
        /// </summary>
        /// <param name="name">The GameObject name containing the tooth number.</param>
        /// <returns>The extracted tooth key as an integer.</returns>
        private int GetTeeth(string name)
        {
            return int.Parse(Regex.Replace(name, "[^0-9]", ""));
        }

    }
}
