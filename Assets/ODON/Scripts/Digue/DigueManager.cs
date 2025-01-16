using UnityEngine;

public class DigueManager : MonoBehaviour
{
    [Header("Root Point")]
    [Tooltip("The point at which the digue will start. If null, the root will default to this GameObject's transform.")]
    [SerializeField] private Transform rootPoint;
    [SerializeField] private Transform[] rootParentsPoints;
    [SerializeField] private Transform[] rootRigsPoints;

    [Header("Limit Distance")]
    [Tooltip("The maximum allowed distance between any two points. If exceeded, the digue will reset to the root point.")]
    [Range(0f, 0.5f)]
    [SerializeField] private float limitsDistance = 0.2f;

    [SerializeField] private int nbrParentPoints;
    [SerializeField] private int nbrRigsPoints;
    [SerializeField] private Transform[] parentsPoints;
    [SerializeField] private Transform[] rigsPoints;

    void Start()
    {
        // Ensure rootPoint is set
        if (rootPoint == null)
        {
            rootPoint = transform;
            Debug.Log("Root point defaulted to the GameObject's transform.");
        }

        // Initialize root parent points
        nbrParentPoints = transform.childCount;
        rootParentsPoints = new Transform[nbrParentPoints];

        for (int i = 0; i < nbrParentPoints; i++)
        {
            rootParentsPoints[i] = transform.GetChild(i);
            Debug.Log($"Root Parent Point {i}: {rootParentsPoints[i].name}, Position: {rootParentsPoints[i].position}");
        }

        // Initialize parent points
        nbrParentPoints = transform.childCount;
        parentsPoints = new Transform[nbrParentPoints];

        for (int i = 0; i < nbrParentPoints; i++)
        {
            parentsPoints[i] = transform.GetChild(i);
            Debug.Log($"Parent Point {i}: {parentsPoints[i].name}, Position: {parentsPoints[i].position}");
        }

        // Initialize root rigs points
        if (parentsPoints.Length == 0 || parentsPoints[0] == null)
        {
            Debug.LogWarning("No parent points found or parentsPoints[0] is null.");
            return;
        }

        nbrRigsPoints = parentsPoints[0].childCount;
        rootRigsPoints = new Transform[nbrRigsPoints];

        for (int i = 0; i < nbrRigsPoints; i++)
        {
            rootRigsPoints[i] = parentsPoints[0].GetChild(i);
            Debug.Log($"Root Rig Point {i}: {rootRigsPoints[i].name}, Position: {rootRigsPoints[i].position}");
        }

        UpdateRigsPosition();
        CalculateDistances();
    }

    void Update()
    {
        UpdateRigsPosition();
        float checkDistance = CalculateDistances();
        if (checkDistance > limitsDistance)
        {
            ResetDigue();
        }
    }

    public void UpdateRigsPosition()
    {
        if (parentsPoints.Length == 0 || parentsPoints[0] == null)
        {
            Debug.LogWarning("No parent points found or parentsPoints[0] is null.");
            return;
        }

        nbrRigsPoints = parentsPoints[0].childCount;
        rigsPoints = new Transform[nbrRigsPoints];

        for (int i = 0; i < nbrRigsPoints; i++)
        {
            rigsPoints[i] = parentsPoints[0].GetChild(i);
            Debug.Log($"Child Point {i}: {rigsPoints[i].name}, Position: {rigsPoints[i].position}");
        }
    }

    public float CalculateDistances()
    {
        if (rigsPoints == null || rigsPoints.Length == 0)
        {
            Debug.LogWarning("No points available to calculate distances.");
            return 0.0f;
        }

        float totalDistance = 0;
        int pairCount = 0;

        for (int i = 0; i < rigsPoints.Length; i++)
        {
            if (rigsPoints[i] == null) continue;

            for (int j = i + 1; j < rigsPoints.Length; j++)
            {
                if (rigsPoints[j] == null) continue;

                totalDistance += Vector3.Distance(rigsPoints[i].position, rigsPoints[j].position);
                pairCount++;
            }
        }

        float averageDistance = pairCount > 0 ? totalDistance / pairCount : 0;
        Debug.Log($"Average Distance Between Points: {averageDistance}");
        return averageDistance;
    }

    public void ResetDigue()
    {
        if (rootPoint == null)
        {
            Debug.LogError("Root point is null! Cannot reset digue.");
            return;
        }

        transform.position = rootPoint.position;
        transform.rotation = rootPoint.rotation;

        for (int i = 0; i < nbrParentPoints; i++)
        {
            parentsPoints[i] = rootParentsPoints[i];
        }
        for (int i = 0; i < nbrRigsPoints; i++)
        {
            rigsPoints[i] = rootRigsPoints[i];
        }

        Debug.Log("Digue reset to root point.");
    }
}
