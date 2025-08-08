namespace UnityEngine.InputSystem.Switch.LowLevel
{
    public static class Vector3Extensions
    {
        public static bool IsActuated(this Vector3 currentAngularVelocity, ref Vector3 lastAngularVelocity, float threshold = 0.01f)
        {
            float delta = (currentAngularVelocity - lastAngularVelocity).magnitude;

            if (delta >= threshold)
            {
                lastAngularVelocity = currentAngularVelocity;
                return true;
            }

            return false;
        }
    }
}
