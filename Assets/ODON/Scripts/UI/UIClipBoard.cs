using UnityEngine;
using ODON.Data;
using TMPro;

namespace ODON.UI
{
    public class UIClipBoard : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject checkMale;
        [SerializeField] private GameObject checkFemale;
        [SerializeField] private GameObject checkLatexTrue;
        [SerializeField] private GameObject checkLatexFalse;
        [SerializeField] private GameObject checkNormalPose;
        [SerializeField] private GameObject checParachutePose;
        [SerializeField] private GameObject checkInlayCore;
        [SerializeField] private GameObject checkCastCrown;
        [SerializeField] private GameObject checkMetalCeramicCrown;
        [SerializeField] private GameObject checkStellite;
        [SerializeField] private GameObject checkResinPartial;
        [SerializeField] private GameObject checkZirconia;

        [SerializeField] private TextMeshProUGUI patientNameText;
        [SerializeField] private TextMeshProUGUI patientAgeText;
        [SerializeField] private TextMeshProUGUI patientTeintedToothText;

        [SerializeField] private GameObject toothRoot;
        [SerializeField] private GameObject[][] checkToothSection = 
        {
            new GameObject[8],
            new GameObject[8],
            new GameObject[8],
            new GameObject[8]
        };
        [SerializeField] private TextMeshProUGUI patientNumberToothText;


        private void Awake()
        {
            InitializeToothSections();
        }

        void OnEnable()
        {
            GameManager.GameHandler.Instance.onUpdatePatientData.AddListener(UpdateUI);
        }

        void OnDisable()
        {
            GameManager.GameHandler.Instance.onUpdatePatientData.RemoveListener(UpdateUI);
        }

        public void UpdateUI(PatientData data)
        {
            SetNamePatient(data.PatientName);
            SetAgePatient(data.Age);
            SetGenderCheck(data.Gender);

            SetTeintedToothText(data.TeintedTooth);
            SetAllergiesCheck(data.HasLatexAllergy);
            SetNormalPoseCheck(data.HasNormalPose);
            checkInlayCore.SetActive(data.InlayCore);
            checkCastCrown.SetActive(data.CastCrown);
            checkMetalCeramicCrown.SetActive(data.MetalCeramicCrown);
            checkStellite.SetActive(data.Stellite);
            checkResinPartial.SetActive(data.ResinPartial);
            checkZirconia.SetActive(data.Zirconia);

            HighlightTreatedTooth(data.TreatedToothWithSection);
            patientNumberToothText.text = data.TreatedToothWithSection.ToString();
        }

        private void SetNamePatient(string name)
        {
            patientNameText.text = name;
        }
        private void SetAgePatient(int age)
        {
            patientAgeText.text = age.ToString();
        }
        private void SetToothSection(int section, int treatedTooth)
        {
            if (section < 1 || section > 4 || treatedTooth < 1 || treatedTooth > 8)
            {
                Debug.LogError("Invalid tooth section or treated tooth number.");
                return;
            }
            patientNumberToothText.text = $"{section}{treatedTooth}";
        }
        private void SetTeintedToothText(TeintedThooth teintedTooth)
        {
            patientTeintedToothText.text = teintedTooth.ToString();
        }

        private void SetGenderCheck(Gender gender)
        {
            checkMale.SetActive(gender == Gender.Male);
            checkFemale.SetActive(gender == Gender.Female);
        }

        private void SetNormalPoseCheck(bool hasNormalPose)
        {
            if (hasNormalPose)
            {
                checkNormalPose.SetActive(true);
                checParachutePose.SetActive(false);
            }
            else
            {
                checParachutePose.SetActive(true);
                checkNormalPose.SetActive(false);
            }
        }

        private void SetAllergiesCheck(bool hasLatexAllergy)
        {
            if (hasLatexAllergy)
            {
                checkLatexTrue.SetActive(true);
                checkLatexFalse.SetActive(false);
            }
            else
            {
                checkLatexFalse.SetActive(true);
                checkLatexTrue.SetActive(false);
            }
        }


        private void InitializeToothSections()
        {
            if (toothRoot.transform.childCount != 4)
            {
                Debug.LogError("Tooth root must have exactly 4 quadrant children.");
                return;
            }

            for (int q = 0; q < 4; q++)
            {
                Transform quadrant = toothRoot.transform.GetChild(q);
                if (quadrant.childCount < 8)
                {
                    Debug.LogWarning($"Quadrant {q + 1} has less than 8 teeth.");
                }

                for (int t = 0; t < 8 && t < quadrant.childCount; t++)
                {
                    checkToothSection[q][t] = quadrant.GetChild(t).gameObject;
                }
            }
        }


        private void HighlightTreatedTooth(int toothNumber)
        {
            int quadrant = (toothNumber / 10) - 1;
            int index = (toothNumber % 10) - 1;
            if (quadrant >= 0 && quadrant < 4 && index >= 0 && index < 8)
            {
                checkToothSection[quadrant][index]?.SetActive(true);
            }
        }
    }
}