using UnityEngine;
using ODON.Data;
using TMPro;

namespace ODON.UI
{
    /// <summary>
    /// Manages the UI clipboard displaying patient information in the ODON application.
    /// Handles updating UI elements for patient name, age, gender, allergies, dental work, and treated tooth highlighting.
    /// </summary>
    public class UIClipBoard : MonoBehaviour
    {
        /// <summary>
        /// UI element for male gender check.
        /// </summary>
        [Header("UI Elements")]
        [SerializeField] private GameObject checkMale;

        /// <summary>
        /// UI element for female gender check.
        /// </summary>
        [SerializeField] private GameObject checkFemale;

        /// <summary>
        /// UI element for latex allergy present.
        /// </summary>
        [SerializeField] private GameObject checkLatexTrue;

        /// <summary>
        /// UI element for latex allergy absent.
        /// </summary>
        [SerializeField] private GameObject checkLatexFalse;

        /// <summary>
        /// UI element for normal pose.
        /// </summary>
        [SerializeField] private GameObject checkNormalPose;

        /// <summary>
        /// UI element for parachute pose.
        /// </summary>
        [SerializeField] private GameObject checParachutePose;

        /// <summary>
        /// UI element for inlay core dental work.
        /// </summary>
        [SerializeField] private GameObject checkInlayCore;

        /// <summary>
        /// UI element for cast crown dental work.
        /// </summary>
        [SerializeField] private GameObject checkCastCrown;

        /// <summary>
        /// UI element for metal ceramic crown dental work.
        /// </summary>
        [SerializeField] private GameObject checkMetalCeramicCrown;

        /// <summary>
        /// UI element for stellite dental work.
        /// </summary>
        [SerializeField] private GameObject checkStellite;

        /// <summary>
        /// UI element for resin partial dental work.
        /// </summary>
        [SerializeField] private GameObject checkResinPartial;

        /// <summary>
        /// UI element for zirconia dental work.
        /// </summary>
        [SerializeField] private GameObject checkZirconia;

        /// <summary>
        /// Text field for displaying patient name.
        /// </summary>
        [SerializeField] private TextMeshProUGUI patientNameText;

        /// <summary>
        /// Text field for displaying patient age.
        /// </summary>
        [SerializeField] private TextMeshProUGUI patientAgeText;

        /// <summary>
        /// Text field for displaying patient tooth color.
        /// </summary>
        [SerializeField] private TextMeshProUGUI patientTeintedToothText;

        /// <summary>
        /// Root GameObject containing tooth quadrants.
        /// </summary>
        [SerializeField] private GameObject toothRoot;

        /// <summary>
        /// 2D array of GameObjects for tooth section checkboxes, organized by quadrant and tooth index.
        /// </summary>
        [SerializeField] private GameObject[][] checkToothSection =
        {
            new GameObject[8],
            new GameObject[8],
            new GameObject[8],
            new GameObject[8]
        };

        /// <summary>
        /// Text field for displaying treated tooth number.
        /// </summary>
        [SerializeField] private TextMeshProUGUI patientNumberToothText;

        /// <summary>
        /// Initializes tooth section references on Awake.
        /// </summary>
        private void Awake()
        {
            InitializeToothSections();
        }

        /// <summary>
        /// Registers the UpdateUI listener for patient data updates.
        /// </summary>
        void OnEnable()
        {
            GameManager.GameHandler.Instance.onUpdatePatientData.AddListener(UpdateUI);
        }

        /// <summary>
        /// Unregisters the UpdateUI listener for patient data updates.
        /// </summary>
        void OnDisable()
        {
            GameManager.GameHandler.Instance.onUpdatePatientData.RemoveListener(UpdateUI);
        }

        /// <summary>
        /// Updates all UI elements based on the provided patient data.
        /// </summary>
        /// <param name="data">The patient data to display.</param>
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

        /// <summary>
        /// Sets the patient name in the UI.
        /// </summary>
        /// <param name="name">The patient's name.</param>
        private void SetNamePatient(string name)
        {
            patientNameText.text = name;
        }

        /// <summary>
        /// Sets the patient age in the UI.
        /// </summary>
        /// <param name="age">The patient's age.</param>
        private void SetAgePatient(int age)
        {
            patientAgeText.text = age.ToString();
        }

        /// <summary>
        /// Sets the tooth section text in the UI.
        /// </summary>
        /// <param name="section">Quadrant section number.</param>
        /// <param name="treatedTooth">Treated tooth index.</param>
        private void SetToothSection(int section, int treatedTooth)
        {
            if (section < 1 || section > 4 || treatedTooth < 1 || treatedTooth > 8)
            {
                Debug.LogError("Invalid tooth section or treated tooth number.");
                return;
            }
            patientNumberToothText.text = $"{section}{treatedTooth}";
        }

        /// <summary>
        /// Sets the tooth color text in the UI.
        /// </summary>
        /// <param name="teintedTooth">The patient's tooth color.</param>
        private void SetTeintedToothText(TeintedThooth teintedTooth)
        {
            patientTeintedToothText.text = teintedTooth.ToString();
        }

        /// <summary>
        /// Sets the gender checkboxes in the UI.
        /// </summary>
        /// <param name="gender">The patient's gender.</param>
        private void SetGenderCheck(Gender gender)
        {
            checkMale.SetActive(gender == Gender.Male);
            checkFemale.SetActive(gender == Gender.Female);
        }

        /// <summary>
        /// Sets the pose checkboxes in the UI.
        /// </summary>
        /// <param name="hasNormalPose">True if the patient has a normal pose.</param>
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

        /// <summary>
        /// Sets the allergy checkboxes in the UI.
        /// </summary>
        /// <param name="hasLatexAllergy">True if the patient has a latex allergy.</param>
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

        /// <summary>
        /// Initializes the tooth section references from the toothRoot hierarchy.
        /// </summary>
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

        /// <summary>
        /// Highlights the treated tooth in the UI by activating the corresponding checkbox.
        /// </summary>
        /// <param name="toothNumber">The treated tooth number.</param>
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