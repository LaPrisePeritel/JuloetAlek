using System;
using UnityEngine;

namespace TPCamera
{
    [Serializable]
    public class CameraConfiguration


    {
        public float Yaw;
        public float Pitch;
        public float Roll;
        public float Distance;
        public float Fov;

        public Vector3 Pivot;

        public Vector3 GetPosition() => Pivot;
        public Quaternion GetRotation() => Quaternion.Euler(Pitch, Yaw, Roll);

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(Pivot, 0.25f);
            Vector3 position = GetPosition();
            Gizmos.DrawLine(Pivot, position);
            Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, Fov, 0.5f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}
