using UnityEngine;
using UnityEngine.Events;

public class ArmatureDigueBehaviour : MonoBehaviour
{
    [Header("Joint Settings")]
    public JointSettings jointSettings;

    [Header("Event System")]
    public UnityEvent eventDropDam;

    [Header("Joints Main")]
    public GameObject jointX;
    public GameObject jointY;
    public GameObject joint_X;
    public GameObject joint_Y;
    public GameObject jointXY;
    public GameObject jointX_Y;
    public GameObject joint_XY;
    public GameObject joint_X_Y;

    [Header("Joints Hole")]
    public GameObject Hole;

    [Header("Item destroy")]
    public GameObject[] setFalse;

    public void InitJoints()
    {
        GameObject[] joints = new GameObject[]
        {
            jointX, jointY, joint_X, joint_Y, jointXY, jointX_Y, joint_XY, joint_X_Y, 
        };

        SetRigidbodyConfiguartion(Hole);

        MakeEssentialJoints(joints);
        MakeConnectionLinks(joints, jointSettings);
        MakeGrabbableJoint(joints);

        foreach (GameObject item in setFalse)
        {
            item.SetActive(false);
        }
    }

    private void MakeEssentialJoints(GameObject[] joints)
    {
        // Setup Rigidbody for each joint
        foreach (GameObject jointGO in joints)
        {
            if (jointGO == null) continue;
            
            BoxCollider col = jointGO.GetComponent<BoxCollider>();
            if (col == null)
            {
                col = jointGO.AddComponent<BoxCollider>();
                col.size = Vector3.one * jointSettings.BoxColliderSize;
            }

            SetRigidbodyConfiguartion(jointGO);
        }
    }

    private void SetRigidbodyConfiguartion(GameObject obj){
        Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
                rb = obj.AddComponent<Rigidbody>();

            rb.mass = jointSettings.rbMass;
            rb.isKinematic = false;
    }

    private void MakeConnectionLinks(GameObject[] joints, JointSettings jointSettings)
    {
        foreach (GameObject jointGO in joints)
        {
            if (jointGO == null) continue;

            GameObject[] connections = GetConnectionsForJoint(jointGO);

            foreach (GameObject target in connections)
            {
                if (target == null) continue;

                ConfigurableJoint joint = jointGO.AddComponent<ConfigurableJoint>();

                if (jointSettings != null)
                {
                    jointSettings.ApplyTo(joint);
                    joint.connectedBody = target.GetComponent<Rigidbody>(); 
                }
            }
        }
    }

    // Méthode pour définir manuellement les connexions souhaitées
    private GameObject[] GetConnectionsForJoint(GameObject jointGO)
    {
        if (jointGO == jointX) return new GameObject[] { jointXY, jointX_Y, Hole};
        if (jointGO == jointY) return new GameObject[] { jointXY, joint_XY, Hole };
        if (jointGO == joint_X) return new GameObject[] { joint_XY, joint_X_Y, Hole};
        if (jointGO == joint_Y) return new GameObject[] { jointX_Y, joint_X_Y, Hole };
        if (jointGO == jointXY) return new GameObject[] { jointX, jointY};
        if (jointGO == jointX_Y) return new GameObject[] { jointX, joint_Y};
        if (jointGO == joint_XY) return new GameObject[] { joint_X, jointY};
        if (jointGO == joint_X_Y) return new GameObject[] { joint_X, joint_Y};

        return new GameObject[0];
    }

    private void MakeGrabbableJoint(GameObject[] joints){
        foreach (GameObject jointGO in joints)
        {
            if (jointGO == null) continue;

            SetObjectGrabable sOG = jointGO.GetComponent<SetObjectGrabable>();
            if (sOG == null)
            {
                jointGO.AddComponent<SetObjectGrabable>();
            }

        }
    }
}
