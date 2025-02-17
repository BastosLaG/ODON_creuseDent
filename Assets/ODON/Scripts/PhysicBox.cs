using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicBox : MonoBehaviour
{
    public Vector3 offset; // Offset from the object's position in world space
    public Vector3 boxSize = new(1, 1, 1); // Size of the box to cast
    [SerializeField] private string hitStringContain = "Cube";
    [SerializeField] private UnityEvent<GameObject> OnBoxEnter, OnBoxExit;
    private HashSet<Collider> collidedObjects = new();

    // Update is called once per frame
    private void Update()
    {
        // Cast the box at the desired world position
        PerformBoxCast();
    }

    private void PerformBoxCast()
    {
        // Calculate the world position of the box cast origin
        Vector3 worldPosition = transform.position + offset;

        // Perform the BoxCastAll to detect multiple collisions
        RaycastHit[] hits = Physics.BoxCastAll(worldPosition, boxSize / 2, boxSize.normalized, Quaternion.identity, 0);

        // Create a set to store the current colliders
        HashSet<Collider> currentColliders = new();

        // If there are hits
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                // Get the collider and add it to the current colliders set
                Collider hitCollider = hit.collider;
                _ = currentColliders.Add(hitCollider);

                // Check if the object is one you want to interact with
                if (!collidedObjects.Contains(hitCollider) && hitCollider.gameObject.name.Contains(hitStringContain))
                {
                    OnBoxEnter.Invoke(hitCollider.gameObject);
                }
            }
        }

        // Now check for exit events by comparing previous and current colliders
        foreach (Collider previousCollider in collidedObjects)
        {
            if (!currentColliders.Contains(previousCollider))
            {
                // If the collider is no longer in the current set, it's an exit event
                OnColliderExit(previousCollider);
            }
        }

        // Update the collided objects set for the next frame
        collidedObjects = currentColliders;

        // Debugging: visualize the box cast in the scene view
        Debug.DrawRay(worldPosition, offset / 2, Color.red);
    }

    // Function to handle the exit event of a collider
    private void OnColliderExit(Collider collider)
    {
        // You can perform any logic here when a collider exits
        if (collider.gameObject.name.Contains(hitStringContain))
        {
            OnBoxExit.Invoke(collider.gameObject);
        }
    }


    // Draw the box cast in the scene view
    private void OnDrawGizmos()
    {
        // Calculate the world position of the box cast origin
        Vector3 worldPosition = transform.position + offset;

        // Set the color for the Gizmos
        Gizmos.color = Color.green;

        // Draw a wireframe cube to visualize the box size and position
        Gizmos.DrawWireCube(worldPosition, boxSize);
    }
}
