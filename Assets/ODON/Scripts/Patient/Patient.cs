using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ODON
{
    /// <summary>
    /// Manages the patient character in the ODON application.
    /// Handles movement between points, sitting/lying actions, and state transitions based on patient state.
    /// </summary>
    public class Patient : MonoBehaviour
    {
        /// <summary>
        /// Reference to the patient GameObject when on bed.
        /// </summary>
        public GameObject patientOnBed;

        /// <summary>
        /// Serializable class representing a movement point for the patient.
        /// Contains the target transform, sit/lay flags, and an event for when the point is reached.
        /// </summary>
        [System.Serializable]
        private class Point
        {
            /// <summary>
            /// The transform of the movement point.
            /// </summary>
            public Transform pointTransform;
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
        [SerializeField] private bool isFollowing = false;

        /// <summary>
        /// Indicates if the patient should sit at the next point.
        /// </summary>
        [SerializeField] private bool sitTo = false;

        /// <summary>
        /// Indicates if the patient should sit on the bed.
        /// </summary>
        [SerializeField] private bool sitOnBed = false;

        /// <summary>
        /// The current target transform for movement.
        /// </summary>
        [SerializeField] private Transform currentTarget;

        /// <summary>
        /// Index of the current movement point.
        /// </summary>
        [SerializeField] private int actualPointIndex = 0;

        /// <summary>
        /// The desired patient state.
        /// </summary>
        [SerializeField] private Data.PatientState patientState;

        /// <summary>
        /// The current patient state.
        /// </summary>
        [SerializeField] private Data.PatientState currentPatientState;

        /// <summary>
        /// Initializes the patient, sets up references, and sets initial visibility.
        /// </summary>
        void Start()
        {
            patientOnBed.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);

            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// Updates the patient state and triggers movement to the appropriate point.
        /// </summary>
        void Update()
        {
            patientState = GameManager.GameHandler.Instance.PatientState;
            if (patientState != currentPatientState)
            {
                currentPatientState = patientState;
                if (currentPatientState == Data.PatientState.InWaitingRoom)
                {
                    GoToPoint(0);
                }
                else if (currentPatientState == Data.PatientState.InCabinet)
                {
                    GoToPoint(1);
                }
                else if (currentPatientState == Data.PatientState.InBed)
                {
                    GoToPoint(2);
                }
            }
        }

        /// <summary>
        /// Checks if the patient has reached the destination and triggers sit/lay actions if needed.
        /// </summary>
        void FixedUpdate()
        {
            if (isFollowing && (!agent.pathPending) && (agent.remainingDistance - 0.5f <= agent.stoppingDistance))
            {
                isFollowing = false;
                agent.isStopped = true;
                points[actualPointIndex].OnPointReached.Invoke();
                if (anim != null) anim.SetBool("Walking", false);
                if (sitTo) { sitTo = false; StartCoroutine(Sit(0.25f)); }
            }
        }

        /// <summary>
        /// Coroutine for sitting action, moves and rotates the patient to the target over a duration.
        /// </summary>
        /// <param name="duration">Duration of the sit animation.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        private IEnumerator Sit(float duration)
        {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            anim.SetTrigger("Sit");
            transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, currentTarget.position, timeElapsed / duration);
                transform.rotation = Quaternion.Slerp(startRotation, currentTarget.rotation, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.SetPositionAndRotation(currentTarget.position, currentTarget.rotation);
            Destroy(currentTarget.GetComponent<NavMeshObstacle>());
            agent.SetDestination(transform.position);
            if (sitOnBed)
            {
                patientOnBed.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Moves the patient to the specified point index, triggers sit/lay if needed.
        /// </summary>
        /// <param name="index">Index of the point to move to.</param>
        public void GoToPoint(int index)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            patientOnBed.SetActive(false);
            actualPointIndex = index;
            Point point = points[index];
            if (point.sitTo || point.layTo) { SitTo(point.pointTransform, point.layTo); }
            else { Follow(point.pointTransform); }
        }

        /// <summary>
        /// Makes the patient follow the specified target transform.
        /// </summary>
        /// <param name="target">The target transform to follow.</param>
        public void Follow(Transform target)
        {
            if (agent != null)
            {
                agent.isStopped = false;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                currentTarget = target;
                agent.SetDestination(currentTarget.position);
                if (anim != null)
                {
                    anim.SetTrigger("Follow");
                    anim.SetBool("Walking", true);
                }
                isFollowing = true;
            }
        }

        /// <summary>
        /// Makes the patient sit to the specified target, optionally on the bed.
        /// </summary>
        /// <param name="target">The target transform to sit at.</param>
        /// <param name="isOnBed">If true, sit on the bed.</param>
        public void SitTo(Transform target, bool isOnBed = false)
        {
            sitTo = true;
            Follow(target);
            AddNavMeshObstacle();
            sitOnBed = isOnBed;
        }

        /// <summary>
        /// Adds a NavMeshObstacle component to the current target for navigation purposes.
        /// </summary>
        private void AddNavMeshObstacle()
        {
            NavMeshObstacle obs = currentTarget.gameObject.AddComponent<NavMeshObstacle>();
            if (obs != null)
            {
                obs.shape = NavMeshObstacleShape.Capsule;
                obs.center = new Vector3(0, 0.5f, -0.75f);
                obs.radius = 0.35f;
                obs.height = 1;
                obs.carving = true;
            }
        }
    }
}