using UnityEngine;

namespace TPCamera
{
    public class FixedView : AView
    {
        [Range(0, 360)]
        public float Yaw;

        [Range(-90, 90)]
        public float Pitch;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, 179)]
        public float Fov;

        public override CameraConfiguration GetConfiguration() => new (Yaw, Pitch, Roll, 0, Fov, transform.position);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1.0f);
            Vector3 position = transform.position;
            Gizmos.DrawLine(transform.position, position);
            Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.Euler(Pitch, Yaw, Roll), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, Fov, 5.0f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}

