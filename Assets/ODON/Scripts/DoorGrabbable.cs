using UnityEngine;

public class DoorGrabbable : MonoBehaviour
{
    public Transform handler;
    
    public void ApplyForceAtGrabPoint()
    {
        transform.position = handler.position;
        transform.rotation = handler.rotation;
        transform.localScale = handler.localScale;
    }
}
