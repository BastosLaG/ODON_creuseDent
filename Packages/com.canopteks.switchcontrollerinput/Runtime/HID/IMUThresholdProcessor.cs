using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    public class IMUThresholdProcessor
    {
        private const float kGyroSensitivity = 0.070f;
        private const float kAccelSensitivity = 0.000244f;

        private float adaptiveThreshold = 0.01f;

        private Vector3 gyroNoiseMean = Vector3.zero;
        private Vector3 gyroNoiseDeviation = Vector3.zero;

        private Vector3 lastControlGyro = Vector3.zero;
        public ref Vector3 LastControlGyro => ref lastControlGyro;

        private readonly Queue<Vector3> gyroThresholdBuffer = new();
        private readonly int bufferSize = 100;
        public bool isRecording = false;

        public Vector3 UncalibratedThresholdAcceleration(IMUData raw)
        {
            Vector3 thresholdAcceleration = new Vector3(raw.accelX, raw.accelY, raw.accelZ) * kAccelSensitivity;
            thresholdAcceleration.z += 1.0f;
            return thresholdAcceleration;
        }

        public Vector3 UncalibratedThresholdGyro(IMUData raw)
        {
            return new Vector3(raw.gyro1, raw.gyro2, raw.gyro3) * kGyroSensitivity;
        }

        public void FeedGyroSample(Vector3 sample)
        {
            if (gyroThresholdBuffer.Count >= bufferSize)
                gyroThresholdBuffer.Dequeue();
            gyroThresholdBuffer.Enqueue(sample);
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

            // Calculate deviation (absolute average difference from mean)
            Vector3 deviationSum = Vector3.zero;
            foreach (var item in gyroThresholdBuffer)
                deviationSum += new Vector3(
                    Mathf.Abs(item.x - gyroNoiseMean.x),
                    Mathf.Abs(item.y - gyroNoiseMean.y),
                    Mathf.Abs(item.z - gyroNoiseMean.z)
                );

            gyroNoiseDeviation = deviationSum / (gyroThresholdBuffer.Count-1);

            // Adaptive threshold = average magnitude of deviation
            adaptiveThreshold = gyroNoiseDeviation.magnitude;

            isRecording = false;
            Debug.Log($"Calibration complete. Noise mean: {gyroNoiseMean}, deviation: {gyroNoiseDeviation}, adaptive threshold: {adaptiveThreshold}");
        }

        public bool IsActuatedGyro(Vector3 currentAngularVelocity)
        {
            float delta = (currentAngularVelocity - lastControlGyro).magnitude;
            if (delta >= adaptiveThreshold)
            {
                lastControlGyro = currentAngularVelocity;
                return true;
            }
            return false;
        }

        public int GetThresholdSampleCount() => gyroThresholdBuffer.Count;
        public int GetBufferSize() => bufferSize;
    }
}
