using System;
using UnityEngine;

/// <summary>
/// Solves inverse kinematics for a foot in the ODON application.
/// Handles foot placement, stepping logic, and synchronization with the other foot for realistic movement over terrain.
/// </summary>
[Obsolete("IKFootSolver is deprecated")]
public class IKFootSolver : MonoBehaviour
{
    /// <summary>
    /// Indicates if the foot is currently moving forward.
    /// </summary>
    public bool isMovingForward;

    /// <summary>
    /// Layer mask used for terrain detection.
    /// </summary>
    [SerializeField] LayerMask terrainLayer = default;

    /// <summary>
    /// Reference to the body transform for foot positioning.
    /// </summary>
    [SerializeField] Transform body = default;

    /// <summary>
    /// Reference to the other foot solver for synchronization.
    /// </summary>
    [SerializeField] IKFootSolver otherFoot = default;

    /// <summary>
    /// Speed of the foot step transition.
    /// </summary>
    [SerializeField] float speed = 4;

    /// <summary>
    /// Minimum distance required to trigger a new step.
    /// </summary>
    [SerializeField] float stepDistance = .2f;

    /// <summary>
    /// Length of a forward step.
    /// </summary>
    [SerializeField] float stepLength = .2f;

    /// <summary>
    /// Length of a side step.
    /// </summary>
    [SerializeField] float sideStepLength = .1f;

    /// <summary>
    /// Height of the foot during a step.
    /// </summary>
    [SerializeField] float stepHeight = .3f;

    /// <summary>
    /// Offset applied to the foot position.
    /// </summary>
    [SerializeField] Vector3 footOffset = default;

    /// <summary>
    /// Offset applied to the foot rotation.
    /// </summary>
    public Vector3 footRotOffset;

    /// <summary>
    /// Vertical offset applied to the foot position.
    /// </summary>
    public float footYPosOffset = 0.1f;

    /// <summary>
    /// Vertical offset for the ray start position.
    /// </summary>
    public float rayStartYOffset = 0;

    /// <summary>
    /// Length of the ray used for terrain detection.
    /// </summary>
    public float rayLength = 1.5f;

    /// <summary>
    /// Spacing of the foot relative to the body.
    /// </summary>
    float footSpacing;

    /// <summary>
    /// Previous position of the foot.
    /// </summary>
    Vector3 oldPosition, currentPosition, newPosition;

    /// <summary>
    /// Previous normal of the foot.
    /// </summary>
    Vector3 oldNormal, currentNormal, newNormal;

    /// <summary>
    /// Interpolation value for step transition.
    /// </summary>
    float lerp;

    /// <summary>
    /// Initializes foot positions and normals.
    /// </summary>
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    /// <summary>
    /// Updates foot position, rotation, and handles stepping logic each frame.
    /// </summary>
    void Update()
    {
        transform.position = currentPosition + Vector3.up * footYPosOffset;
        transform.localRotation = Quaternion.Euler(footRotOffset);

        Ray ray = new Ray(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        Debug.DrawRay(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, rayLength, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                Vector3 direction = Vector3.ProjectOnPlane(info.point - currentPosition, Vector3.up).normalized;

                float angle = Vector3.Angle(body.forward, body.InverseTransformDirection(direction));

                isMovingForward = angle < 50 || angle > 130;

                if (isMovingForward)
                {
                    newPosition = info.point + direction * stepLength + footOffset;
                    newNormal = info.normal;
                }
                else
                {
                    newPosition = info.point + direction * sideStepLength + footOffset;
                    newNormal = info.normal;
                }
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    /// <summary>
    /// Draws a gizmo sphere at the new foot position for debugging.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }

    /// <summary>
    /// Returns true if the foot is currently moving (step in progress).
    /// </summary>
    /// <returns>True if the foot is moving, otherwise false.</returns>
    public bool IsMoving()
    {
        return lerp < 1;
    }
}
