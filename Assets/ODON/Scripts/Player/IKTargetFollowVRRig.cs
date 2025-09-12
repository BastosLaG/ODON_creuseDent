using System;
using UnityEngine;

[Serializable, Obsolete("VRMap is deprecated")]
/// <summary>
/// Represents a mapping between a VR target and an IK target, including position and rotation offsets.
/// </summary>
public class VRMap
{
    /// <summary>
    /// The transform of the VR target (e.g., headset or controller).
    /// </summary>
    public Transform vrTarget;

    /// <summary>
    /// The transform of the IK target to be driven.
    /// </summary>
    public Transform ikTarget;

    /// <summary>
    /// Position offset applied when mapping the VR target to the IK target.
    /// </summary>
    public Vector3 trackingPositionOffset;

    /// <summary>
    /// Rotation offset applied when mapping the VR target to the IK target.
    /// </summary>
    public Vector3 trackingRotationOffset;

    /// <summary>
    /// Maps the VR target's position and rotation to the IK target, applying offsets.
    /// </summary>
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
[Obsolete("IKTargetFollowVRRig is deprecated")]
/// <summary>
/// Follows the VR rig targets and applies IK mapping for head and hands in the ODON application.
/// Handles position and rotation smoothing for the player body.
/// </summary>
public class IKTargetFollowVRRig : MonoBehaviour
{
    /// <summary>
    /// Smoothing factor for body rotation when following the head yaw.
    /// </summary>
    [Range(0,1)]
    public float turnSmoothness = 0.1f;

    /// <summary>
    /// VRMap for the head target.
    /// </summary>
    public VRMap head;

    /// <summary>
    /// VRMap for the left hand target.
    /// </summary>
    public VRMap leftHand;

    /// <summary>
    /// VRMap for the right hand target.
    /// </summary>
    public VRMap rightHand;

    /// <summary>
    /// Position offset for the body relative to the head.
    /// </summary>
    public Vector3 headBodyPositionOffset;

    /// <summary>
    /// Yaw offset for the body rotation relative to the head.
    /// </summary>
    public float headBodyYawOffset;

    /// <summary>
    /// Updates the body position and rotation based on the head target, and maps all VR targets to IK targets.
    /// </summary>
    void LateUpdate()
    {
        transform.position = head.ikTarget.position + headBodyPositionOffset;
        float yaw = head.vrTarget.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
