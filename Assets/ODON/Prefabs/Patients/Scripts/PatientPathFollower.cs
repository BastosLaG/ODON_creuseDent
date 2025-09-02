    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PatientPathFollower : MonoBehaviour
{
    [System.Serializable]
    private class Point
    {
        public Transform[] pointTransforms;
        public bool sitTo, layTo;
        public UnityEvent OnPointReached;
    }
    [SerializeField] private List<Point> points;
    private NavMeshAgent agent;
    private Animator anim;

    private bool isFollowing = false, sitTo = false, sitOnBed = false;
    private Point currentPoint = null;
    private int actualPointIndex = 0, pointTransformIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void GoToPoint(int index)
    {
        actualPointIndex = index;
        Point point = points[index];
        // choisis une destination aléatoire si le point en possède plusieurs.
        pointTransformIndex = Random.Range(0, point.pointTransforms.Length-1);
        sitTo = point.sitTo;
        sitOnBed = point.layTo;
        Follow(point);
    }

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

    private IEnumerator Stand()
    {
        // Si le patient est assis, attend qu'il se lève avant de bouger.
        string name = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToUpper(); // 0 = layer index
        while (!name.Contains("WALK"))
        {
            name = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToUpper();
            yield return new WaitForEndOfFrame();
        }
        // Definition de la destination.
        Vector3 destination = currentPoint.pointTransforms[pointTransformIndex].position;
        agent.enabled = true;
        agent.SetDestination(destination);
        isFollowing = true;
    }

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

    private IEnumerator Sit(float duration)
    {
        agent.enabled = false;
        if (currentPoint.layTo) anim.SetTrigger("LieOn");
        else anim.SetTrigger("Sit");
        transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        float timeElapsed = 0f;

        Vector3 destination = currentPoint.pointTransforms[pointTransformIndex].position;
        Vector3 angularAngle = currentPoint.pointTransforms[pointTransformIndex].eulerAngles;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, timeElapsed / duration);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(angularAngle), timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(destination, Quaternion.Euler(angularAngle));
    }
}
