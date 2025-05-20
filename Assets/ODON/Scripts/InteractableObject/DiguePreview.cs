using UnityEngine;
using System.Collections;

public class DiguePreview : MonoBehaviour
{
    [SerializeField] private Transform DigueFinalTransform {get; set;}
    [SerializeField] private Material finalMat {get; set;}
    [SerializeField] private float minDistance = 2.0f;

    private Coroutine DiguePreviewCoroutine = null;

    private static int handsHolding = 0;
    private static int activePreviewCoroutines = 0;

    public static int ActivePreviewCount => activePreviewCoroutines;

    public void OnSelectEnter()
    {
        handsHolding++;

        if (DiguePreviewCoroutine == null)
        {
            DiguePreviewCoroutine = StartCoroutine(CompareDistancesCoroutine(0.5f));
            activePreviewCoroutines++;
        }
    }

    public void OnSelectExit()
    {
        handsHolding = Mathf.Max(0, handsHolding - 1);

        if (handsHolding == 0)
        {
            if (DiguePreviewCoroutine != null)
            {
                StopCoroutine(DiguePreviewCoroutine);
                DiguePreviewCoroutine = null;
                activePreviewCoroutines--;
            }

            PlaceObject();
        }
    }

    private IEnumerator CompareDistancesCoroutine(float compareInterval)
    {
        while (true)
        {
            CompareDistance();
            yield return new WaitForSeconds(compareInterval);
        }
    }

    private void CompareDistance()
    {
        float distance = Vector3.Distance(transform.position, DigueFinalTransform.position);

        DigueFinalTransform.gameObject.SetActive(distance < minDistance);
    }

    public void PlaceObject()
    {
        float distance = Vector3.Distance(transform.position, DigueFinalTransform.position);
        if (distance < minDistance)
        {
            DigueFinalTransform.GetComponentInChildren<SkinnedMeshRenderer>().material = finalMat;

            Transform root = transform;
            for (int i = 0; i < 3 && root.parent != null; i++)
            {
                root = root.parent;
            }

            root.gameObject.SetActive(false);
        }
    }

    public void SetMaterial(Material material)
    {
        finalMat = material;
    }
    public void SetTransform(Transform transform)
    {
        DigueFinalTransform = transform;
    }
}
