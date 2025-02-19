using UnityEngine;

public class DigueManager : MonoBehaviour
{
    [Header("Root Point")]
    [Tooltip("The point at which the digue will start. If null, the root will default to this GameObject's transform.")]
    [SerializeField] private Transform rootPoint;
    [SerializeField] private Transform[] rootParentsPoints;
    [SerializeField] private Transform[] rootJoints;

    [Header("Limit Distance")]
    [Tooltip("The maximum allowed distance between any two points. If exceeded, the digue will reset to the root point.")]
    [Range(0f, 1.0f)]
    [SerializeField] private float limitsDistance = 0.4f;

    [SerializeField] private int nbrParentPoints;
    [SerializeField] private int nbrJoints;
    [SerializeField] private Transform[] parentsPoints;
    [SerializeField] private Transform[] joints;


    [Header("Debug")]
    [SerializeField] private bool debugger = false;
    [SerializeField] private bool debugDistanceCalculated = false;

    void Start()
    {

        // Ensure rootPoint is set
        if (rootPoint == null)
        {
            rootPoint = transform;
            if (debugger)
                Debug.Log("Root point defaulted to the GameObject's transform.");
        }

        // Initialize root parent points
        nbrParentPoints = transform.childCount;
        rootParentsPoints = new Transform[nbrParentPoints];

        for (int i = 0; i < nbrParentPoints; i++)
        {
            rootParentsPoints[i] = transform.GetChild(i);
            if (debugger)
                Debug.Log($"Root Parent Point {i}: {rootParentsPoints[i].name}, Position: {rootParentsPoints[i].position}");
        }

        // Initialize parent points
        nbrParentPoints = transform.childCount;
        parentsPoints = new Transform[nbrParentPoints];

        for (int i = 0; i < nbrParentPoints; i++)
        {
            parentsPoints[i] = transform.GetChild(i);
            if (debugger)
                Debug.Log($"Parent Point {i}: {parentsPoints[i].name}, Position: {parentsPoints[i].position}");
        }

        // Initialize root rigs points
        if (parentsPoints.Length == 0 || parentsPoints[0] == null)
        {
            if (debugger)
                Debug.LogWarning("No parent points found or parentsPoints[0] is null.");
            return;
        }

        nbrJoints = parentsPoints[0].childCount;
        rootJoints = new Transform[nbrJoints];

        for (int i = 0; i < nbrJoints; i++)
        {
            rootJoints[i] = parentsPoints[0].GetChild(i);
            if (debugger)
                Debug.Log($"Root Rig Point {i}: {rootJoints[i].name}, Position: {rootJoints[i].position}");
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
            if (debugger)
                Debug.LogWarning("No parent points found or parentsPoints[0] is null.");
            return;
        }

        nbrJoints = parentsPoints[0].childCount;
        joints = new Transform[nbrJoints];

        for (int i = 0; i < nbrJoints; i++)
        {
            joints[i] = parentsPoints[0].GetChild(i);
            if (debugger)
                Debug.Log($"Child Point {i}: {joints[i].name}, Position: {joints[i].position}");
        }
    }

    public float CalculateDistances()
    {
        if (joints == null || joints.Length == 0)
        {
            if (debugger)
                Debug.LogWarning("No rigs points available for distance calculation.");
            return 0.0f;
        }

        float maxDistance = 0;

        for (int i = 0; i < joints.Length; i++)
        {
            if (joints[i] == null) continue;

            for (int j = i + 1; j < joints.Length; j++)
            {
                if (joints[j] == null) continue;

                float distance = Vector3.Distance(joints[i].position, joints[j].position);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
        }

        if (debugger || debugDistanceCalculated)
            Debug.Log($"Maximum Distance Between Points: {maxDistance}");
        return maxDistance;
    }


    public void ResetDigue()
    {
        if (rootPoint == null)
        {
            if (debugger)
                Debug.LogError("Root point is null! Cannot reset digue.");
            return;
        }

        transform.position = rootPoint.position;
        transform.rotation = rootPoint.rotation;

        for (int i = 0; i < nbrParentPoints; i++)
        {
            parentsPoints[i] = rootParentsPoints[i];
        }
        for (int i = 0; i < nbrJoints; i++)
        {
            joints[i] = rootJoints[i];
        }

        if (debugger)
            Debug.Log("Digue reset to root point.");
    }
}
