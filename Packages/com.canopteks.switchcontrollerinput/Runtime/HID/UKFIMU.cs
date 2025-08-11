using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using UnityEngine;

namespace UnityEngine.InputSystem.Switch.LowLevel
{
    public class UKFIMU
    {

        private Quaternion orientation = Quaternion.identity;
        private Vector3 gyroBias = Vector3.zero;

        private readonly int n = 4; // dimension de l'état (quaternion)
        private Vector<float> x; // état courant (vecteur 4D)
        private Matrix<float> P; // covariance d'état
        private Matrix<float> Q; // bruit processus
        private Matrix<float> R; // bruit mesure

        private Matrix<float> sigmaPointsPred; // sigma points après propagation (prédiction)

        private readonly float alpha = 1e-3f;
        private readonly float beta = 2f;
        private readonly float kappa = 0f;

        private readonly float lambda;
        private readonly float gamma;

        private Vector<float> Wm; // poids moyenne
        private Vector<float> Wc; // poids covariance

        public Vector3 LastGyro { get; private set; }
        public Vector3 LastAccel { get; private set; }

        public UKFIMU()
        {
            // État initial : quaternion identité (w,x,y,z)
            x = DenseVector.OfArray(new float[] { 1, 0, 0, 0 });

            P = DenseMatrix.CreateIdentity(n) * 0.1f;
            Q = DenseMatrix.CreateIdentity(n) * 0.01f;  // par exemple 0.001 → 0.01
            R = DenseMatrix.CreateIdentity(3) * 0.05f;  // 0.01 → 0.05


            lambda = alpha * alpha * (n + kappa) - n;
            gamma = Mathf.Sqrt(n + lambda);

            Wm = DenseVector.Create(2 * n + 1, 1f / (2 * (n + lambda)));
            Wc = DenseVector.Create(2 * n + 1, 1f / (2 * (n + lambda)));

            Wm[0] = lambda / (n + lambda);
            Wc[0] = Wm[0] + (1 - alpha * alpha + beta);
        }

        // Génération des sigma points selon la méthode UKF classique
        private Matrix<float> GenerateSigmaPoints()
        {
            // Symétrisation
            P = 0.5f * (P + P.Transpose());

            // Projection positive définie
            P = MakePositiveDefinite(P);

            // Jitter diagonal
            for (int i = 0; i < P.RowCount; i++)
                P[i, i] += 1e-4f;

            var sqrtP = P.Cholesky().Factor;

            var sigmaPoints = DenseMatrix.Create(n, 2 * n + 1, 0);
            sigmaPoints.SetColumn(0, x);

            for (int i = 0; i < n; i++)
            {
                var col = sqrtP.Column(i) * gamma;
                sigmaPoints.SetColumn(i + 1, x + col);
                sigmaPoints.SetColumn(i + 1 + n, x - col);
            }

            return sigmaPoints;
        }


        private Matrix<float> MakePositiveDefinite(Matrix<float> mat, float minEigenvalue = 1e-4f)
        {
            var evd = mat.Evd();
            var D = evd.D.Clone();
            var V = evd.EigenVectors;

            for (int i = 0; i < D.RowCount; i++)
            {
                if (D[i, i] < minEigenvalue)
                {
                    D[i, i] = minEigenvalue;
                }
            }

            var fixedMat = V * D * V.Transpose();
            fixedMat = 0.5f * (fixedMat + fixedMat.Transpose()); // symétriser

            return fixedMat;
        }


        // Multiplication de quaternions (Unity utilise (x,y,z,w), attention à l'ordre)
        private Quaternion QuaternionMultiply(Quaternion q1, Quaternion q2)
        {
            // Cette fonction utilise la convention w,x,y,z
            return new Quaternion(
                q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x,
                q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
            );
        }

        // Conversion Vector<float> (w,x,y,z) vers Quaternion (x,y,z,w)
        private Quaternion VectorToQuaternion(Vector<float> v)
        {
            return new Quaternion(v[1], v[2], v[3], v[0]);
        }

        // Conversion Quaternion vers Vector<float> (w,x,y,z)
        private Vector<float> QuaternionToVector(Quaternion q)
        {
            return DenseVector.OfArray(new float[] { q.w, q.x, q.y, q.z });
        }

        // Propagation d'état selon modèle gyroscope (Euler)
        private Vector<float> StateTransition(Vector<float> state, Vector3 gyro, float deltaTime)
        {
            Quaternion q = VectorToQuaternion(state);

            // Créer omega quaternion à partir du vecteur gyro (rad/s)
            Quaternion omega = new Quaternion(gyro.x, gyro.y, gyro.z, 0f);

            // dq/dt = 0.5 * q * omega
            Quaternion qDot = QuaternionMultiply(q, omega);
            qDot.x *= 0.5f;
            qDot.y *= 0.5f;
            qDot.z *= 0.5f;
            qDot.w *= 0.5f;

            // Intégration Euler : q_new = q + qDot * dt
            Quaternion qNew = new(
                q.x + qDot.x * deltaTime,
                q.y + qDot.y * deltaTime,
                q.z + qDot.z * deltaTime,
                q.w + qDot.w * deltaTime
            );

            qNew.Normalize();

            return QuaternionToVector(qNew);
        }

        public void Predict(Vector3 gyro, float deltaTime)
        {
            LastGyro = gyro;
            var sigmaPoints = GenerateSigmaPoints(); // à partir de x et P

            // Propagation de chaque sigma point
            for (int i = 0; i < sigmaPoints.ColumnCount; i++)
            {
                var sp = sigmaPoints.Column(i);
                var spProp = StateTransition(sp, gyro, deltaTime);
                sigmaPoints.SetColumn(i, spProp);
            }

            // Stocker pour Update
            sigmaPointsPred = sigmaPoints.Clone();

            // Calcul moyenne pondérée
            Vector<float> xPred = DenseVector.Create(n, 0);
            for (int i = 0; i < sigmaPoints.ColumnCount; i++)
                xPred += Wm[i] * sigmaPoints.Column(i);

            Quaternion qPred = VectorToQuaternion(xPred);
            qPred.Normalize();
            xPred = QuaternionToVector(qPred);

            // Calcul covariance prédite
            Matrix<float> PPred = DenseMatrix.Create(n, n, 0);
            for (int i = 0; i < sigmaPoints.ColumnCount; i++)
            {
                var diff = sigmaPoints.Column(i) - xPred;
                PPred += Wc[i] * diff.ToColumnMatrix() * diff.ToRowMatrix();
            }
            PPred += Q;

            x = xPred;
            P = PPred;

            P = 0.5f * (P + P.Transpose());
            for (int i = 0; i < P.RowCount; i++)
                P[i, i] += 1e-9f;

        }

        public void Update(Vector3 accel)
        {
            if (sigmaPointsPred == null)
            {
                Debug.LogError("Call Predict before Update");
                return;
            }

            LastAccel = accel;

            int m = 3;
            var Z = DenseMatrix.Create(m, 2 * n + 1, 0);

            // Fonction d'observation à partir des sigma points prédits (non régénérés)
            for (int i = 0; i < sigmaPointsPred.ColumnCount; i++)
            {
                Quaternion q = VectorToQuaternion(sigmaPointsPred.Column(i));
                Vector3 g = new(0, 0, 1);
                Vector3 rotatedG = q * g;
                Z.SetColumn(i, DenseVector.OfArray(new float[] { rotatedG.x, rotatedG.y, rotatedG.z }));
            }

            // Moyenne pondérée de la mesure
            Vector<float> zPred = DenseVector.Create(m, 0);
            for (int i = 0; i < Z.ColumnCount; i++)
                zPred += Wm[i] * Z.Column(i);

            // Covariance innovation et covariance croisée
            Matrix<float> S = DenseMatrix.Create(m, m, 0);
            Matrix<float> Pxz = DenseMatrix.Create(n, m, 0);

            for (int i = 0; i < Z.ColumnCount; i++)
            {
                var zDiff = Z.Column(i) - zPred;
                var xDiff = sigmaPointsPred.Column(i) - x;

                S += Wc[i] * zDiff.ToColumnMatrix() * zDiff.ToRowMatrix();
                Pxz += Wc[i] * xDiff.ToColumnMatrix() * zDiff.ToRowMatrix();
            }

            S += R;

            var K = Pxz * S.Solve(DenseMatrix.CreateIdentity(m));

            var y = DenseVector.OfArray(new float[] { accel.x, accel.y, accel.z }) - zPred;

            x += K * y;

            Quaternion qUpdated = VectorToQuaternion(x);
            qUpdated.Normalize();
            x = QuaternionToVector(qUpdated);

            P -= K * S * K.Transpose();

            P = 0.5f * (P + P.Transpose());
            for (int i = 0; i < P.RowCount; i++)
                P[i, i] += 1e-9f;

            sigmaPointsPred = null;
        }

        public Quaternion GetOrientation()
        {
            return VectorToQuaternion(x);
        }
    }
}
