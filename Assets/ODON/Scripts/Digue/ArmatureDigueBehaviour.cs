using System;
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
    public GameObject jointHoleX;
    public GameObject jointHoleY;
    public GameObject jointHole_X;
    public GameObject jointHole_Y;
    public GameObject jointHoleXY;
    public GameObject jointHoleX_Y;
    public GameObject jointHole_XY;
    public GameObject jointHole_X_Y;

    public GameObject[] setFalse;

    public void InitJoints()
    {
        GameObject[] joints = new GameObject[]
        {
            jointX, jointY, joint_X, joint_Y, jointXY, jointX_Y, joint_XY, joint_X_Y, 
        };

        GameObject[] holeJoints = new GameObject[]
        {
            jointHoleX, jointHoleY, jointHole_X, jointHole_Y, jointHoleXY, jointHoleX_Y, jointHole_XY, jointHole_X_Y
        };

        MakeEssentialJoints(joints);
        MakeConnectionLinks(joints);
        MakeGrabbableJoint(joints);

        MakeEssentialJoints(holeJoints);
        MakeConnectionLinks(holeJoints);

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
                col.size = Vector3.one * 0.05f;
            }

            Rigidbody rb = jointGO.GetComponent<Rigidbody>();
            if (rb == null)
                rb = jointGO.AddComponent<Rigidbody>();

            rb.mass = 0.1f;
            rb.isKinematic = false;
        }
    }
    private void MakeConnectionLinks(GameObject[] joints)
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
        if (jointGO == jointX) return new GameObject[] { jointXY, jointX_Y, jointHoleX};
        if (jointGO == jointY) return new GameObject[] { jointXY, joint_XY, jointHoleY };
        if (jointGO == joint_X) return new GameObject[] { joint_XY, joint_X_Y, jointHole_X};
        if (jointGO == joint_Y) return new GameObject[] { jointX_Y, joint_X_Y, jointHole_Y };
        if (jointGO == jointXY) return new GameObject[] { jointX, jointY, jointHoleXY };
        if (jointGO == jointX_Y) return new GameObject[] { jointX, joint_Y, jointHoleX_Y};
        if (jointGO == joint_XY) return new GameObject[] { joint_X, jointY, jointHole_XY};
        if (jointGO == joint_X_Y) return new GameObject[] { joint_X, joint_Y, jointHole_X_Y};
        if (jointGO == jointHoleX) return new GameObject[] { jointHoleXY, jointHoleX_Y, jointX};
        if (jointGO == jointHoleY) return new GameObject[] { jointHoleXY, jointHole_XY, jointY };
        if (jointGO == jointHole_X) return new GameObject[] { jointHole_X_Y, jointHole_XY, joint_X};
        if (jointGO == jointHole_Y) return new GameObject[] { jointHoleX_Y, jointHole_X_Y, joint_Y };
        if (jointGO == jointHoleXY) return new GameObject[] { jointHoleY, jointHoleX, jointXY };
        if (jointGO == jointHoleX_Y) return new GameObject[] { jointHoleX, jointHole_Y, jointX_Y};
        if (jointGO == jointHole_XY) return new GameObject[] { jointHole_X, jointHoleY, joint_XY };
        if (jointGO == jointHole_X_Y) return new GameObject[] { jointHole_X, jointHole_Y, joint_X_Y };
        // Si aucune connexion n'est trouvée, retourne un tableau vide
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
