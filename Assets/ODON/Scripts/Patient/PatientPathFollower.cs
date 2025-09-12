using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Controls the movement and actions of a patient character along a path in the ODON application.
/// Handles navigation, sitting/lying actions, and event invocation when points are reached.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PatientPathFollower : MonoBehaviour
{
    /// <summary>
    /// Serializable class representing a movement point for the patient.
    /// Contains multiple possible transforms, sit/lay flags, and an event for when the point is reached.
    /// </summary>
    [System.Serializable]
    private class Point
    {
        /// <summary>
        /// Array of possible transforms for the movement point.
        /// </summary>
        public Transform[] pointTransforms;
        /// <summary>
        /// Indicates if the patient should sit at this point.
        /// </summary>
        public bool sitTo;
        /// <summary>
        /// Indicates if the patient should lay at this point.
        /// </summary>
        public bool layTo;
        /// <summary>
        /// Event invoked when the point is reached.
        /// </summary>
        public UnityEvent OnPointReached;
    }

    /// <summary>
    /// List of movement points for the patient.
    /// </summary>
    [SerializeField] private List<Point> points;

    /// <summary>
    /// Reference to the NavMeshAgent component for movement.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Reference to the Animator component for animations.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Indicates if the patient is currently following a path.
    /// </summary>
    private bool isFollowing = false;

    /// <summary>
    /// Indicates if the patient should sit at the next point.
    /// </summary>
    private bool sitTo = false;

    /// <summary>
    /// Indicates if the patient should sit on the bed.
    /// </summary>
    private bool sitOnBed = false;

    /// <summary>
    /// The current movement point being followed.
    /// </summary>
    private Point currentPoint = null;

    /// <summary>
    /// Index of the current movement point.
    /// </summary>
    private int actualPointIndex = 0;

    /// <summary>
    /// Index of the selected transform for the current movement point.
    /// </summary>
    private int pointTransformIndex = 0;

    /// <summary>
    /// Initializes the NavMeshAgent and Animator components.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Moves the patient to the specified point index, randomly selecting a destination if multiple transforms exist.
    /// </summary>
    /// <param name="index">Index of the point to move to.</param>
    public void GoToPoint(int index)
    {
        actualPointIndex = index;
        Point point = points[index];
        // Chooses a random destination if the point has multiple transforms.
        pointTransformIndex = Random.Range(0, point.pointTransforms.Length-1);
        sitTo = point.sitTo;
        sitOnBed = point.layTo;
        Follow(point);
    }

    /// <summary>
    /// Makes the patient follow the specified movement point.
    /// </summary>
    /// <param name="point">The movement point to follow.</param>
    private void Follow(Point point)
    {
        if (agent != null)
        {
            currentPoint = point;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            if (anim != null) anim.SetBool("Walking", true);
            StartCoroutine(Stand());
        }
    }

    /// <summary>
    /// Coroutine that waits for the patient to stand before moving to the destination.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator Stand()
    {
        // If the patient is sitting, wait until they stand before moving.
        string name = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToUpper(); // 0 = layer index
        while (!name.Contains("WALK"))
        {
            name = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToUpper();
            yield return new WaitForEndOfFrame();
        }
        // Set the destination.
        Vector3 destination = currentPoint.pointTransforms[pointTransformIndex].position;
        agent.enabled = true;
        agent.SetDestination(destination);
        isFollowing = true;
    }

    /// <summary>
    /// Checks if the patient has reached the destination and triggers sit/lay actions if needed.
    /// </summary>
    void Update()
    {
        if (isFollowing && (!agent.pathPending) && (agent.remainingDistance - 0.01f <= agent.stoppingDistance))
        {
            isFollowing = false;
            points[actualPointIndex].OnPointReached.Invoke();
            if (anim != null) anim.SetBool("Walking", false);
            if (sitTo || sitOnBed) { sitTo = false; sitOnBed = false; StartCoroutine(Sit(0.25f)); }
        }
    }

    /// <summary>
    /// Coroutine for sitting or lying action, moves and rotates the patient to the target over a duration.
    /// </summary>
    /// <param name="duration">Duration of the sit/lie animation.</param>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator Sit(float duration)
    {
        agent.enabled = false;
        if (currentPoint.layTo)
        {
            anim.SetTrigger("LieOn");
        }
        else
        {
            anim.SetTrigger("Sit");
        }
        transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        float timeElapsed = 0f;

        Vector3 destination = currentPoint.pointTransforms[pointTransformIndex].position;
        Vector3 angularAngle = currentPoint.pointTransforms[pointTransformIndex].eulerAngles;

        while (timeElapsed < duration)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, destination, timeElapsed / duration),
                Quaternion.Slerp(startRotation, Quaternion.Euler(angularAngle), timeElapsed / duration)
                );
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(destination, Quaternion.Euler(angularAngle));
    }
}
