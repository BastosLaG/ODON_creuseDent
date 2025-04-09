using UnityEngine;

[CreateAssetMenu(fileName = "JointSettings", menuName = "ScriptableObjects/JointSettings", order = 1)]
public class JointSettings : ScriptableObject
{
    public ConfigurableJointMotion xMotion;
    public ConfigurableJointMotion yMotion;
    public ConfigurableJointMotion zMotion;

    public ConfigurableJointMotion angularXMotion;
    public ConfigurableJointMotion angularYMotion;
    public ConfigurableJointMotion angularZMotion;

    public SoftJointLimitSpring linearLimitSpring;
    public SoftJointLimit linearLimit;

    public JointDrive xDrive;
    public JointDrive yDrive;
    public JointDrive zDrive;

    public Vector3 targetPosition;
    public Vector3 targetVelocity;

    public JointDrive slerpDrive;
    public Quaternion targetRotation;

    public JointProjectionMode projectionMode;
    public float projectionDistance;
    public float projectionAngle;

    public bool autoConfigureConnectedAnchor;
    public Vector3 anchor;
    public Vector3 connectedAnchor;

    public void ApplyTo(ConfigurableJoint joint)
    {
        joint.xMotion = xMotion;
        joint.yMotion = yMotion;
        joint.zMotion = zMotion;

        joint.angularXMotion = angularXMotion;
        joint.angularYMotion = angularYMotion;
        joint.angularZMotion = angularZMotion;

        joint.linearLimitSpring = linearLimitSpring;
        joint.linearLimit = linearLimit;

        joint.xDrive = xDrive;
        joint.yDrive = yDrive;
        joint.zDrive = zDrive;

        joint.targetPosition = targetPosition;
        joint.targetVelocity = targetVelocity;

        joint.slerpDrive = slerpDrive;
        joint.targetRotation = targetRotation;

        joint.projectionMode = projectionMode;
        joint.projectionDistance = projectionDistance;
        joint.projectionAngle = projectionAngle;

        joint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
        joint.anchor = anchor;
        joint.connectedAnchor = connectedAnchor;
    }
}
