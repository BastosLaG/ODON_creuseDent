using UnityEngine;

public class DoorGrabbable : MonoBehaviour
{
    public Transform handler;

    public bool IsActiveCheckpointProgressEnabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool IsLocked { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // void ISendActiveCheckpointProgress.SendActiveCheckpointProgress()
    // {
    //     IsActiveCheckpointProgressEnabled = true;
    // }

    public void ApplyForceAtGrabPoint()
    {
        transform.position = handler.position;
        transform.rotation = handler.rotation;
        transform.localScale = handler.localScale;
    }
}
