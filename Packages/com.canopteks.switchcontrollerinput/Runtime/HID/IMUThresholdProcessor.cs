using System.Collections.Generic;
using UnscentedKalmanFilter;
using UnityEngine;

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    public class IMUThresholdProcessor
    {
        private const float kGyroSensitivity = 0.070f;
        private const float kAccelSensitivity = 0.000244f;

        private readonly float adaptiveThreshold = .05f;

        private Vector3 gyroNoiseMean = Vector3.zero;
        private Vector3 gyroNoiseDeviation = Vector3.zero;

        private Vector3 lastControlGyro = Vector3.zero;
        public ref Vector3 LastControlGyro => ref lastControlGyro;

        private readonly Queue<Vector3> gyroThresholdBuffer = new();
        private readonly int bufferSize = 100;
        public bool isRecording = false;


        UKF filterZ = new();

        List<double> measurementsZ = new();
        List<double> estimationsZ = new();

        public Vector3 UncalibratedThresholdAcceleration(IMUData raw)
        {
            Vector3 thresholdAcceleration = new Vector3(raw.accelX, raw.accelY, raw.accelZ) * kAccelSensitivity;
            thresholdAcceleration.z += 1.0f;
            return thresholdAcceleration;
        }

        public Vector3 UncalibratedThresholdGyro(IMUData raw)
        {
            Vector3 uncalibratedThresholdGyro = new Vector3(raw.gyro1 - gyroNoiseMean.x,
                                                            raw.gyro2 - gyroNoiseMean.y,
                                                            raw.gyro3 - gyroNoiseMean.z) * kGyroSensitivity;

            uncalibratedThresholdGyro.x = IsInBound(uncalibratedThresholdGyro.x);
            uncalibratedThresholdGyro.y = IsInBound(uncalibratedThresholdGyro.y);
            uncalibratedThresholdGyro.z = IsInBound(uncalibratedThresholdGyro.z);

            return uncalibratedThresholdGyro;
        }

        public float IsInBound(float value)
        {
            if (-adaptiveThreshold <= value && value <= adaptiveThreshold)
            {
                return 0f;
            }
            return value;
        }

        public void FeedGyroSample(Vector3 sample)
        {
            if (gyroThresholdBuffer.Count >= bufferSize)
                gyroThresholdBuffer.Dequeue();
            gyroThresholdBuffer.Enqueue(sample);

            // Use only Z-axis for UKF
            double[] measurement = { sample.z };
            filterZ.Update(measurement);

            // Store raw and filtered data for analysis
            if (measurementsZ.Count >= bufferSize)
            {
                measurementsZ.RemoveAt(0);
                estimationsZ.RemoveAt(0);
            }
            measurementsZ.Add(measurement[0]);
            estimationsZ.Add(filterZ.getState()[0]);
        }

        public void Calibrate()
        {
            if (gyroThresholdBuffer.Count < bufferSize - 1)
            {
                Debug.LogWarning("Not enough samples to calibrate.");
                return;
            }

            // Calculate mean
            Vector3 sum = Vector3.zero;
            foreach (var item in gyroThresholdBuffer)
                sum += item;
            gyroNoiseMean = sum / gyroThresholdBuffer.Count;

            isRecording = false;
            Debug.Log($"Calibration complete. Noise mean: {gyroNoiseMean}");
        }

        public bool IsActuatedGyro(Vector3 currentAngularVelocity)
        {
            // Get latest UKF estimate for Z-axis
            if (estimationsZ.Count == 0)
                return false;

            float filteredZ = (float)estimationsZ[^1]; // last filtered value
            float lastFilteredZ = lastControlGyro.z;

            float delta = Mathf.Abs(filteredZ - lastFilteredZ);

            if (delta >= adaptiveThreshold)
            {
                // Update last control gyro using filtered value
                lastControlGyro = new Vector3(currentAngularVelocity.x, currentAngularVelocity.y, filteredZ);
                return true;
            }
            return false;
        }

        public int GetThresholdSampleCount() => gyroThresholdBuffer.Count;
        public int GetBufferSize() => bufferSize;
        public float GetLastEstimatedGyroZ() => estimationsZ.Count > 0 ? (float)estimationsZ[^1] : 0f;
    }
}
