using UnityEngine;

public class DoorGrabbable : MonoBehaviour
{
    public Transform handler;
    
    public void ApplyForceAtGrabPoint()
    {
        transform.SetPositionAndRotation(handler.position, handler.rotation);
        transform.localScale = handler.localScale;
    }
}
