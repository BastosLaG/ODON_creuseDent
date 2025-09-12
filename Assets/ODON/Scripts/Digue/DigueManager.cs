using System;
using UnityEngine;

/// <summary>
/// Manages the digue (dam) system in the ODON application.
/// Handles initialization, position updates, distance calculations, and resetting logic for the dam points and joints.
/// </summary>
[Obsolete("DigueManager is deprecated.")]
public class DigueManager : MonoBehaviour
{
    /// <summary>
    /// The point at which the digue will start. If null, defaults to this GameObject's transform.
    /// </summary>
    [Header("Root Point")]
    [Tooltip("The point at which the digue will start. If null, the root will default to this GameObject's transform.")]
    [SerializeField] private Transform rootPoint;

    /// <summary>
    /// Array of root parent points for the digue.
    /// </summary>
    [SerializeField] private Transform[] rootParentsPoints;

    /// <summary>
    /// Array of root joints for the digue.
    /// </summary>
    [SerializeField] private Transform[] rootJoints;

    /// <summary>
    /// The maximum allowed distance between any two points. If exceeded, the digue will reset to the root point.
    /// </summary>
    [Header("Limit Distance")]
    [Tooltip("The maximum allowed distance between any two points. If exceeded, the digue will reset to the root point.")]
    [Range(0f, 1.0f)]
    [SerializeField] private float limitsDistance = 0.4f;

    /// <summary>
    /// Number of parent points.
    /// </summary>
    [SerializeField] private int nbrParentPoints;

    /// <summary>
    /// Number of joints.
    /// </summary>
    [SerializeField] private int nbrJoints;

    /// <summary>
    /// Array of parent points for the digue.
    /// </summary>
    [SerializeField] private Transform[] parentsPoints;

    /// <summary>
    /// Array of joints for the digue.
    /// </summary>
    [SerializeField] private Transform[] joints;

    /// <summary>
    /// Enables debug logging.
    /// </summary>
    [Header("Debug")]
    [SerializeField] private bool debugger = false;

    /// <summary>
    /// Enables debug logging for distance calculations.
    /// </summary>
    [SerializeField] private bool debugDistanceCalculated = false;

    /// <summary>
    /// Initializes the digue manager, sets up points and joints, and performs initial calculations.
    /// </summary>
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

    /// <summary>
    /// Updates the positions of the rig joints and checks distances each frame.
    /// Resets the digue if the maximum distance is exceeded.
    /// </summary>
    void Update()
    {
        UpdateRigsPosition();
        float checkDistance = CalculateDistances();
        if (checkDistance > limitsDistance)
        {
            ResetDigue();
        }
    }

    /// <summary>
    /// Updates the positions of the rig joints based on the parent points.
    /// </summary>
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

    /// <summary>
    /// Calculates the maximum distance between any two joints.
    /// </summary>
    /// <returns>The maximum distance found between joints.</returns>
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

    /// <summary>
    /// Resets the digue to the root point and restores parent points and joints to their initial state.
    /// </summary>
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
