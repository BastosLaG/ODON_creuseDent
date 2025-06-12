using Unity.VisualScripting;
using UnityEngine;

public interface ISendActiveCheckpointProgress
{
    bool IsActiveCheckpointProgressEnabled { get; set; }
    bool IsLocked { get; set; }

    void SendActiveCheckpointProgress(float progress) {
        if (IsLocked) return;
    }
}