using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    unsafe struct SwitchControllerFullInputReport
    {
        public const int kSize = 362;
        public const byte ExpectedReportId = 0x30;

        [FieldOffset(0)] public byte reportId;
        [FieldOffset(1)] public byte timer;
        [FieldOffset(2)] public byte batteryAndConnectionInfo;

        [FieldOffset(3)] public byte rightButtons;

        [FieldOffset(4)] public byte sharedButtons;

        [FieldOffset(5)] public byte leftButtons;

        [FieldOffset(6)] public fixed byte leftStick[3];
        [FieldOffset(9)] public fixed byte rightStick[3];

        [FieldOffset(12)] public byte vibratorInputReport;

        // For 0x21 (Subcommand replies)
        // [FieldOffset(13)] public byte subcommandAck;
        // [FieldOffset(14)] public byte subcommandReplyId;
        // [FieldOffset(15)] public fixed byte subcommandReplyData[35];


        // For 0x23 (NFC/IR MCU FW)
        // [FieldOffset(13)] public fixed byte nfcIRMCUFWDataInputReport[37];

        // For 0x30, 0x31, 0x32, 0x33 (normal mode)
        [FieldOffset(13)] public IMUData imuData0ms;
        [FieldOffset(25)] public IMUData imuData5ms;
        [FieldOffset(37)] public IMUData imuData10ms;

        // For 0x31 (NFC/IR?)
        // [FieldOffset(49)] public fixed byte nfcIRDataInputReport[313];


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SwitchControllerVirtualInputState ToHIDInputReport(
            ref SwitchControllerHID.CalibrationData calibData,
            SpecificControllerTypeEnum controllerType,
            Quaternion currentOrientation,
            IMUThresholdProcessor ukfFilter)
        {
            // --- Lecture des sticks analogiques ---
            Vector2 leftStickVec = Vector2.zero;
            Vector2 rightStickVec = Vector2.zero;

            // Left Stick
            if (controllerType == SpecificControllerTypeEnum.LeftJoyCon ||
                controllerType == SpecificControllerTypeEnum.ProController)
            {
                var lCalib = calibData.lStickCalibData;
                var l0 = leftStick[0];
                var l1 = leftStick[1];
                var l2 = leftStick[2];
                var rawX = l0 | ((l1 & 0xF) << 8);
                var rawY = (l1 >> 4) | (l2 << 4);

                float x = (Mathf.InverseLerp(lCalib.xMin, lCalib.xMax, rawX) * 2) - 1;
                float y = (Mathf.InverseLerp(lCalib.yMin, lCalib.yMax, rawY) * 2) - 1;
                leftStickVec = new Vector2(x, y);
            }

            // Right Stick
            if (controllerType == SpecificControllerTypeEnum.RightJoyCon ||
                controllerType == SpecificControllerTypeEnum.ProController)
            {
                var rCalib = calibData.rStickCalibData;
                var r0 = rightStick[0];
                var r1 = rightStick[1];
                var r2 = rightStick[2];
                var rawX = r0 | ((r1 & 0xF) << 8);
                var rawY = (r1 >> 4) | (r2 << 4);

                float x = (Mathf.InverseLerp(rCalib.xMin, rCalib.xMax, rawX) * 2) - 1;
                float y = (Mathf.InverseLerp(rCalib.yMin, rCalib.yMax, rawY) * 2) - 1;
                rightStickVec = new Vector2(x, y);
            }

            // --- Lecture IMU & UKF ---
            ProcessIMUSample(imuData0ms, ukfFilter.Ukf);
            ProcessIMUSample(imuData5ms, ukfFilter.Ukf);
            ProcessIMUSample(imuData10ms, ukfFilter.Ukf);

            // Orientation estimée par le filtre
            Quaternion fusedOrientation = ukfFilter.Ukf.GetOrientation();

            ukfFilter.FeedGyroSample(fusedOrientation.eulerAngles);

            // --- Création état final ---
            SwitchControllerVirtualInputState state = new SwitchControllerVirtualInputState
            {
                leftStick = leftStickVec,
                rightStick = rightStickVec,
                acceleration = ukfFilter.Ukf.LastAccel,
                angularVelocity = ukfFilter.Ukf.LastGyro,
                orientation = fusedOrientation.eulerAngles
            };

            // --- Boutons ---
            state.Set(SwitchControllerVirtualInputState.Button.Y, (rightButtons & 0x01) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.X, (rightButtons & 0x02) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.B, (rightButtons & 0x04) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.A, (rightButtons & 0x08) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.SR, ((leftButtons | rightButtons) & 0x10) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.SL, ((leftButtons | rightButtons) & 0x20) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.R, (rightButtons & 0x40) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.ZR, (rightButtons & 0x80) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Minus, (sharedButtons & 0x01) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Plus, (sharedButtons & 0x02) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.StickR, (sharedButtons & 0x04) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.StickL, (sharedButtons & 0x08) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Down, (leftButtons & 0x01) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Up, (leftButtons & 0x02) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Right, (leftButtons & 0x04) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.Left, (leftButtons & 0x08) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.L, (leftButtons & 0x40) != 0);
            state.Set(SwitchControllerVirtualInputState.Button.ZL, (leftButtons & 0x80) != 0);

            return state;
        }

        // --- Traitement d’un échantillon IMU ---
        private void ProcessIMUSample(IMUData imu, UKFIMU ukf)
        {
            // Conversion en unités physiques (à adapter selon ton calibrage)
            Vector3 accel = new Vector3(imu.accelX, imu.accelY, imu.accelZ) * 0.00025f;
            Vector3 gyro = new Vector3(imu.gyro1, imu.gyro2, imu.gyro3) * 0.061f; // en deg/s

            // Passage au UKF
            ukf.Predict(gyro, Time.deltaTime);
            ukf.Update(accel);
        }


    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct IMUData
    {
        [FieldOffset(0)] public short accelX;
        [FieldOffset(2)] public short accelY;
        [FieldOffset(4)] public short accelZ;

        [FieldOffset(6)] public short gyro1;
        [FieldOffset(8)] public short gyro2;
        [FieldOffset(10)] public short gyro3;
    }
}