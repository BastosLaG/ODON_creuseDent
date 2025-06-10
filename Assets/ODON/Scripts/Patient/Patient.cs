using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Patient : MonoBehaviour
{
    public GameObject patientOnBed;
    [System.Serializable]
    private class Point
    {
        public Transform pointTransform;
        public bool sitTo, layTo;
        public UnityEvent OnPointReached;
    }
    [SerializeField] private List<Point> points;
    private NavMeshAgent agent;
    private Animator anim;

    private bool isFollowing = false, sitTo = false, sitOnBed = false;
    private Transform currentTarget;
    private int actualPointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // TESTS, TO REPLACE BY OWN CALLS
        StartCoroutine(PathTest());
    }
    private IEnumerator PathTest()
    {
        yield return new WaitForSeconds(2);
        GoToPoint(0); // Go to the chair to give documents
        yield return new WaitForSeconds(10);
        GoToPoint(1); // Go to the bed to get digued
        yield return new WaitForSeconds(5);
        GoToPoint(2); // Leave the cabinet
    }

    public void GoToPoint(int index)
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        patientOnBed.SetActive(false);
        actualPointIndex = index;
        Point point = points[index];
        Debug.Log(point.sitTo + " " + point.layTo);
        if (point.sitTo || point.layTo) { SitTo(point.pointTransform, point.layTo); }
        else { Follow(point.pointTransform); }
    }

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

    public void SitTo(Transform target, bool isOnBed = false)
    {
        sitTo = true;
        Follow(target);
        AddNavMeshObstacle();
        sitOnBed = isOnBed;
    }

    void Update()
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

    private void AddNavMeshObstacle()
    {
        NavMeshObstacle obs = currentTarget.AddComponent<NavMeshObstacle>();
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
