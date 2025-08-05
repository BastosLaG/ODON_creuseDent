using System.Collections.Generic;

namespace UnityEngine.InputSystem.Switch.LowLevel
{ 
    public class IMUThresholdProcessor : MonoBehaviour
    {
        private const float kGyroSensitivity = 0.070f;
        private const float kAccelSensitivity = 0.000244f;

        private const float thresholdDerivationValue = 0.5f;

        private Vector3 gyroMaxSizeThreshold = new(float.MinValue, float.MinValue, float.MinValue);
        private Vector3 gyroMinSizeThreshold = new(float.MaxValue, float.MaxValue, float.MaxValue);

        private readonly Queue<Vector3> gyroThresholdBuffer = new();
        private readonly int bufferSize = 100;

        public bool isRecording = false;

        public Vector3 UncalibratedThresholdAcceleration(IMUData raw)
        {
            return new Vector3(raw.accelX, raw.accelY, raw.accelZ) * kAccelSensitivity;
        }

        public Vector3 UncalibratedThresholdGyro(IMUData raw)
        {
            Vector3 temp = new Vector3(raw.gyro1, raw.gyro2, raw.gyro3) * kGyroSensitivity;
            Vector3 gyro = Vector3.zero;

            if (temp.x < gyroMinSizeThreshold.x || temp.x > gyroMaxSizeThreshold.x)
                gyro.x = temp.x;
            if (temp.y < gyroMinSizeThreshold.y || temp.y > gyroMaxSizeThreshold.y)
                gyro.y = temp.y;
            if (temp.z < gyroMinSizeThreshold.z || temp.z > gyroMaxSizeThreshold.z)
                gyro.z = temp.z;

            return gyro;
        }

        public void FeedGyroSample(Vector3 sample)
        {
            if (gyroThresholdBuffer.Count >= bufferSize)
                gyroThresholdBuffer.Dequeue();

            gyroThresholdBuffer.Enqueue(sample);
        }

        public void Calibrate()
        {
            if (gyroThresholdBuffer.Count < bufferSize)
            {
                Debug.LogWarning("Not enough samples to calibrate.");
                return;
            }

            gyroMaxSizeThreshold = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            gyroMinSizeThreshold = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (var item in gyroThresholdBuffer)
            {
                gyroMaxSizeThreshold = Vector3.Max(gyroMaxSizeThreshold, item);
                gyroMinSizeThreshold = Vector3.Min(gyroMinSizeThreshold, item);
            }

            Vector3 thresholdVec3Derivation = new (thresholdDerivationValue, thresholdDerivationValue, thresholdDerivationValue);

            gyroMaxSizeThreshold += thresholdVec3Derivation;
            gyroMinSizeThreshold -= thresholdVec3Derivation;

            isRecording = false;
            Debug.Log($"Calibration complete. Min: {gyroMinSizeThreshold}, Max: {gyroMaxSizeThreshold}");
        }
    }
}