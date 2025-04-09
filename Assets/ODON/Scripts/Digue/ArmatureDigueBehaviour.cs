using UnityEngine;

public class ArmatureDigueBehaviour : MonoBehaviour
{
    public JointSettings jointSettings;

    public GameObject jointX;
    public GameObject jointY;
    public GameObject joint_X;
    public GameObject joint_Y;
    public GameObject jointXY;
    public GameObject jointX_Y;
    public GameObject joint_XY;
    public GameObject joint_X_Y;

    void Awake()
    {
        InitJoints();
    }

    public void InitJoints()
    {
        GameObject[] joints = new GameObject[]
        {
            jointX, jointY, joint_X, joint_Y, jointXY, jointX_Y, joint_XY, joint_X_Y
        };

        foreach (GameObject jointGO in joints)
        {
            if (jointGO == null) continue;

            // Rigidbody setup
            Rigidbody rb = jointGO.GetComponent<Rigidbody>();
            if (rb == null)
                rb = jointGO.AddComponent<Rigidbody>();

            rb.mass = 0.1f;
            rb.linearDamping = 0.05f;
            rb.angularDamping = 0.05f;
            rb.isKinematic = false;

            // Joint setup
            ConfigurableJoint joint = jointGO.GetComponent<ConfigurableJoint>();
            if (joint == null)
                joint = jointGO.AddComponent<ConfigurableJoint>();

            // Auto-connect to parent if Rigidbody
            Rigidbody parentRB = jointGO.transform.parent?.GetComponent<Rigidbody>();
            if (parentRB != null)
                joint.connectedBody = parentRB;

            if (jointSettings != null)
                jointSettings.ApplyTo(joint);
        }
    }
}
