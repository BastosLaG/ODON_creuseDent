using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    public class IMUThresholdProcessor
    {
        private const float kGyroSensitivity = 0.070f;     // deg/s par unité brute
        private const float kAccelSensitivity = 0.000244f; // g par unité brute

        private Vector3 gyroNoiseMean = Vector3.zero;
        private Vector3 gyroNoiseDeviation = Vector3.zero;

        private Vector3 lastControlGyro = Vector3.zero;
        public ref Vector3 LastControlGyro => ref lastControlGyro;

        private readonly Queue<Vector3> gyroThresholdBuffer = new();
        private readonly int bufferSize = 100;

        public bool IsRecording { get; private set; } = false;
        public bool IsCalibrated { get; private set; } = false;

        // Filtre UKF
        public UKFIMU Ukf { get; set; } = new();
        private float lastUpdateTime = -1f;

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

        /// <summary>
        /// Calibrated + filtré avec UKF → retourne orientation estimée
        /// </summary>
        public Quaternion ProcessIMU(IMUData raw)
        {
            float currentTime = Time.time;
            float deltaTime = (lastUpdateTime < 0f) ? 0f : (currentTime - lastUpdateTime);
            lastUpdateTime = currentTime;

            // Données brutes
            Vector3 rawGyro = UncalibratedThresholdGyro(raw);
            Vector3 rawAccel = UncalibratedThresholdAcceleration(raw);

            // Calibration du gyroscope si dispo
            if (IsCalibrated)
                rawGyro -= gyroNoiseMean;

            // Étape Predict
            Ukf.Predict(rawGyro, deltaTime);

            // Étape Update
            Ukf.Update(rawAccel);

            // Orientation estimée
            return Ukf.GetOrientation();
        }

        public void StartRecording()
        {
            gyroThresholdBuffer.Clear();
            IsRecording = true;
            IsCalibrated = false;
        }

        public void StopAndCalibrate()
        {
            if (gyroThresholdBuffer.Count < bufferSize)
            {
                Debug.LogWarning($"Not enough samples to calibrate. Required: {bufferSize}, got: {gyroThresholdBuffer.Count}");
                IsRecording = false;
                return;
            }

            // Calcul moyenne
            Vector3 sum = Vector3.zero;
            foreach (var sample in gyroThresholdBuffer)
                sum += sample;
            gyroNoiseMean = sum / gyroThresholdBuffer.Count;

            // Déviation absolue moyenne
            Vector3 deviationSum = Vector3.zero;
            foreach (var sample in gyroThresholdBuffer)
            {
                deviationSum += new Vector3(
                    Mathf.Abs(sample.x - gyroNoiseMean.x),
                    Mathf.Abs(sample.y - gyroNoiseMean.y),
                    Mathf.Abs(sample.z - gyroNoiseMean.z)
                );
            }
            gyroNoiseDeviation = deviationSum / gyroThresholdBuffer.Count;

            IsRecording = false;
            IsCalibrated = true;

            Debug.Log($"Calibration complete. Noise mean: {gyroNoiseMean:F4}, deviation: {gyroNoiseDeviation:F4}");
        }

        public void FeedGyroSample(Vector3 sample)
        {
            if (!IsRecording)
                return;

            if (gyroThresholdBuffer.Count >= bufferSize)
                gyroThresholdBuffer.Dequeue();

            gyroThresholdBuffer.Enqueue(sample);
        }

        public bool IsActuatedGyro(Vector3 currentAngularVelocity)
        {
            if (!IsCalibrated)
                return false;

            Vector3 delta = currentAngularVelocity - lastControlGyro;
            if (Mathf.Abs(delta.x) >= gyroNoiseDeviation.x ||
                Mathf.Abs(delta.y) >= gyroNoiseDeviation.y ||
                Mathf.Abs(delta.z) >= gyroNoiseDeviation.z)
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
