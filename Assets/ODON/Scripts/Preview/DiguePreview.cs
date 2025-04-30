using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;

public class DiguePreview : MonoBehaviour
{
    [SerializeField] private Transform DigueFinalTransform;
    private SetObjectGrabable setObjectGrabable;

    private Coroutine cadreEnUPreview = null;

    void Start()
    {
        setObjectGrabable = GetComponent<SetObjectGrabable>();
        if (setObjectGrabable == null)
        {
            setObjectGrabable = gameObject.AddComponent<SetObjectGrabable>();
        }

        // Vérifie que les events ne sont pas null
        if (setObjectGrabable.SelectEnter == null)
            setObjectGrabable.SelectEnter = new UnityEvent();

        if (setObjectGrabable.SelectExit == null)
            setObjectGrabable.SelectExit = new UnityEvent();

        // Ajoute des listeners si non déjà présents
        setObjectGrabable.SelectEnter.AddListener(StartToCompareDistance);
        setObjectGrabable.SelectExit.AddListener(PlaceObject);
    }


    // A appeller quand on attrape l'object pour afficher la pr�visualisation
    public void StartToCompareDistance()
    {
        // ??= -> Change la valeur si elle est nulle, sinon, laisse la valeur par d�faut �quivaut � "if (cadreEnUPreview == null)...".
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

    // Regarde qu'elle preview est la plus proche du object et lui met la pr�visualisation de l'object
    private void CompareDistance()
    {
        float minCadreDistance = Vector3.Distance(transform.position, DigueFinalTransform.position);
        if (minCadreDistance < 0.5f)
        {
            DigueFinalTransform.gameObject.SetActive(true);
        }
    }

    // Active l'objet qui est plac� � l'endroit voulu (quand on relache la gachette)
    public void PlaceObject()
    {
        float minCadreDistance = Vector3.Distance(transform.position, DigueFinalTransform.position);
        if (minCadreDistance < 0.5f)
        {
            DigueFinalTransform.GetComponent<MeshRenderer>().material = gameObject.transform.parent.parent.GetComponentInChildren<MeshRenderer>().material;
            Destroy(gameObject.transform.parent);
        }
    }
}
