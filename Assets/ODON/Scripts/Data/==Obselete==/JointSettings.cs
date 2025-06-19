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

    private SoftJointLimitSpring softJointLimitSpring;
    private SoftJointLimit softJointLimit;
    [Header("Linear Limit Settings")]
    public float linearLimit;
    public float linearLimitSpringNbr;
    public float linearLimitDamperNbr;

    private JointDrive xDrive;
    [Header("Joint Drive X Settings")]
    public float xDriveSpring;
    public float xDriveDamper;
    public float xDriveMaxForce;

    private JointDrive yDrive;
    [Header("Joint Drive Y Settings")]
    public float yDriveSpring;
    public float yDriveDamper;
    public float yDriveMaxForce;

    private JointDrive zDrive;
    [Header("Joint Drive Z Settings")]
    public float zDriveSpring;
    public float zDriveDamper;
    public float zDriveMaxForce;
    
    [Header("Target Settings")]
    public Vector3 targetPosition;
    public Vector3 targetVelocity;

    private JointDrive slerpDrive;
    [Header("Slerp Settings")]
    public float slerpDriveSpring;
    public float slerpDriveDamper;
    public float slerpDriveMaxForce;
    public Quaternion targetRotation;

    public JointProjectionMode projectionMode;
    [Header("Projection Settings")]
    public float projectionDistance;
    public float projectionAngle;

    [Header("Anchors Settings")]
    public bool autoConfigureConnectedAnchor;
    public Vector3 anchor;
    public Vector3 connectedAnchor;

    [Header("RigidiBody")]
    public float rbMass;
    
    [Header("Box Collider")]
    public float BoxColliderSize;

    public void ApplyTo(ConfigurableJoint joint)
    {
        joint.xMotion = xMotion;
        joint.yMotion = yMotion;
        joint.zMotion = zMotion;

        joint.angularXMotion = angularXMotion;
        joint.angularYMotion = angularYMotion;
        joint.angularZMotion = angularZMotion;

        joint.linearLimitSpring = softJointLimitSpring;
        joint.linearLimit = softJointLimit;

        var tempLinearLimitSpring = joint.linearLimitSpring;
        tempLinearLimitSpring.damper = linearLimitDamperNbr;
        tempLinearLimitSpring.spring = linearLimitSpringNbr;
        joint.linearLimitSpring = tempLinearLimitSpring;
        
        xDrive.positionSpring = xDriveSpring;
        xDrive.positionDamper = xDriveDamper;
        xDrive.maximumForce = xDriveMaxForce;
        yDrive.positionSpring = yDriveSpring;
        yDrive.positionDamper = yDriveDamper;
        yDrive.maximumForce = yDriveMaxForce;
        zDrive.positionSpring = zDriveSpring;
        zDrive.positionDamper = zDriveDamper;
        zDrive.maximumForce = zDriveMaxForce;

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
