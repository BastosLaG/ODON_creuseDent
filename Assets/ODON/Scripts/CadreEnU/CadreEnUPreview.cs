using UnityEngine;
using System.Collections;

public class CadreEnUPreview : MonoBehaviour
{
    [SerializeField] private Transform cadreEnUFinalTransform;
    [SerializeField] private Material cadreEnUFinalMat;

    private Coroutine cadreEnUPreview = null;

    // A appeller quand on attrape le crampon pour afficher la prévisualisation
    public void StartToCompareDistance()
    {
        // ??= -> Change la valeur si elle est nulle, sinon, laisse la valeur par défaut équivaut à "if (cadreEnUPreview == null)...".
        cadreEnUPreview ??= StartCoroutine(CompareCadreDistances(0.5f));
    }

    private IEnumerator CompareCadreDistances(float compareInterval)
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
        float minCadreDistance = Vector3.Distance(transform.position, cadreEnUFinalTransform.position);
        if (minCadreDistance < 0.5f)
        {
            cadreEnUFinalTransform.gameObject.SetActive(true);
        }
    }

    // Active l'objet qui est placé à l'endroit voulu (quand on relache la gachette)
    public void PlaceObject()
    {
        float minCadreDistance = Vector3.Distance(transform.position, cadreEnUFinalTransform.position);
        if (minCadreDistance < 0.5f)
        {
            cadreEnUFinalTransform.GetComponent<MeshRenderer>().material = cadreEnUFinalMat;
            Destroy(gameObject);
        }
    }
}
