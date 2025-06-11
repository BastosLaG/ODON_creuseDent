using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    public Transform target; 
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(target.transform.position);
    }
}
