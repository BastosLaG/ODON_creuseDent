using System.Collections;
using UnityEngine;

public class CramponPreview : MonoBehaviour
{
    [SerializeField] private Material sickToothMaterial;
    [SerializeField] private Transform lowerTeethParent;
    [SerializeField] private GameObject cramponPreviewPref;
    [SerializeField] private Vector3 cramponPreviewPosOffset = new Vector3(0, 0.00630000001f, -0.00209999993f);
    [SerializeField] private Vector3[] cramponPreviewRots = new Vector3[16];

    private Transform[] lowerTeeth = new Transform[16];
    private GameObject instantiedCramponPreview = null;
    private Coroutine cramponTeethPreview = null;
    private int toothIndex = -1;


    private void Start()
    {
        // Get all teeth and ignore 2 first objects
        for (int i = 0; i < lowerTeethParent.childCount-2; i++)
        {
            lowerTeeth[i] = lowerTeethParent.GetChild(i+2);
        }
        // Tests
        // Simulation attribution d'une dent malade
        SetSickTooth(31);
        // Simulation prise en main du crampon après 5 secondes
        Invoke(nameof(StartToCompareDistance), 5);
        // Simulation pose du crampon après 30 secondes
        Invoke(nameof(PoseCrampon), 30);
    }

    // Assignation du materiel à la dent malade.
    public void SetSickTooth(int toothNum)
    {
        for (int i = 0; i < lowerTeeth.Length; i++)
        {
            if (lowerTeeth[i].name.Contains(toothNum.ToString()))
            {
                lowerTeeth[i].GetComponent<MeshRenderer>().material = sickToothMaterial;
                return;
            }
        }
    }

    // A appeller quand on attrape le crampon pour afficher la prévisualisation
    public void StartToCompareDistance()
    {
        // ??= -> Change la valeur si elle est nulle, sinon, laisse la valeur par défaut équivaut à "if (cramponTeethPreview == null)...".
        cramponTeethPreview ??= StartCoroutine(CompareTeethDistances(0.5f));
    }

    private IEnumerator CompareTeethDistances(float compareInterval)
    {
        while (true)
        {
            CompareDistance();
            yield return new WaitForSeconds(compareInterval);
        }
    }

    // Regarde qu'elle dent est la plus proche du crampon et lui met la prévisualisation du crampon
    private void CompareDistance()
    {
        float minTeethDistance = Vector3.Distance(transform.position, lowerTeethParent.position);
        if (minTeethDistance < 0.5f)
        {
            int newToothIndex = 0;
            float minDistance = Vector3.Distance(transform.position, lowerTeeth[0].position);
            for (int i = 1; i < lowerTeeth.Length; i++)
            {
                if (Vector3.Distance(transform.position, lowerTeeth[i].position) < minDistance)
                {
                    newToothIndex = i;
                    minDistance = Vector3.Distance(transform.position, lowerTeeth[i].position);
                }
            }
            if (newToothIndex != toothIndex)
            {
                toothIndex = newToothIndex;
                if (instantiedCramponPreview == null) 
                { 
                    instantiedCramponPreview = Instantiate(cramponPreviewPref, Vector3.zero, Quaternion.identity /* TODO : BONNE ROTATION*/, lowerTeeth[toothIndex]);
                    instantiedCramponPreview.transform.localPosition = Vector3.zero;
                    instantiedCramponPreview.transform.localRotation = Quaternion.Euler(cramponPreviewRots[toothIndex]);
                }
                else
                {
                    instantiedCramponPreview.transform.SetParent(lowerTeeth[toothIndex]);
                    instantiedCramponPreview.transform.localPosition = Vector3.zero;
                    instantiedCramponPreview.transform.localRotation = Quaternion.Euler(cramponPreviewRots[toothIndex]);
                }
            }
        }
    }

    // Positione le crampon sur la dent (quand on le relache)
    public void PoseCrampon()
    {
        // Arret de la coroutine de prévisualisation.
        if (cramponTeethPreview != null)
        {
            StopCoroutine(cramponTeethPreview);
            cramponTeethPreview = null;
        }
        instantiedCramponPreview.SetActive(false);
        // TODO : distance de relachement
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(lowerTeeth[toothIndex]);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(cramponPreviewRots[toothIndex]);
    }
}
