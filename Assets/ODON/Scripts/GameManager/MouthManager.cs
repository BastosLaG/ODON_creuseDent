using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace ODON.GameManager
{
    public class MouthManager : MonoBehaviour
    {
        [SerializeField] private static MouthManager instance;
        public static MouthManager Instance => instance;

        [SerializeField] private Dictionary<int, GameObject> teethDictionary = new();
        [SerializeField] private Dictionary<int, GameObject> cramponDictionary = new();
        [SerializeField] private List<CramponPreview> CramponList = new();
        [SerializeField] private GameObject prefabPreviewCrampon;
        [SerializeField] private GameObject prefabCramponInMouth;

        [SerializeField] private UniversalSenderActionToEventManager USATEManagerPutCrampon;

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

            Debug.Log($"teeth needed is the number {GameHandler.Instance.PatientData[GameHandler.Instance.PatientDataIndex].TreatedToothWithSection}");
        }

        public void SwapPrefabs()
        {
            int GoodKey = GameHandler.Instance.PatientData[GameHandler.Instance.PatientDataIndex].TreatedToothWithSection;

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
                        
                        USATEManagerPutCrampon.TryValidateCurrentItem();
                        return;
                    }
                    else
                    {
                        USATEManagerPutCrampon.TryValidateCurrentItem(false);
                        return;
                    }   
                }
            }
        }

        private int GetTeeth(string name)
        {
            return int.Parse(Regex.Replace(name, "[^0-9]", ""));
        }

    }
}
