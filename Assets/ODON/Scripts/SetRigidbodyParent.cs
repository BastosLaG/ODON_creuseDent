using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class SetRigidbodyParent : MonoBehaviour
{
    void Start()
    {
        if (transform.parent.GetComponent<Rigidbody>() != null)
        GetComponent<HingeJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
        else GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
    }

}
