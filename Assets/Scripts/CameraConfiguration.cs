using System;
using UnityEngine;

namespace TPCamera
{
    [Serializable]
    public class CameraConfiguration
    {
        [Range(0, 360)]
        public float Yaw;

        [Range(-90, 90)]
        public float Pitch;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, float.MaxValue)]
        public float Distance;

        [Range(0, 179)]
        public float Fov;

        public Vector3 Pivot;

        public CameraConfiguration(float yaw, float pitch, float roll, float distance, float fov, Vector3 pivot)
        {
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
            Distance = distance;
            Fov = fov;
            Pivot = pivot;
        }
        public CameraConfiguration(CameraConfiguration config)
        {
            Yaw = config.Yaw;
            Pitch = config.Pitch;
            Roll = config.Roll;
            Distance = config.Distance;
            Fov = config.Fov;
            Pivot = config.Pivot;
        }

        public Vector3 GetPosition() => Pivot + (GetRotation() * (Vector3.back * Distance));
        public Quaternion GetRotation() => Quaternion.Euler(Pitch, Yaw, Roll);

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(Pivot, 1.0f);
            Vector3 position = GetPosition();
            Gizmos.DrawLine(Pivot, position);
            Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, Fov, 5.0f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
