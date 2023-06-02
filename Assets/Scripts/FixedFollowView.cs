using UnityEngine;

namespace TPCamera
{
    public class FixedFollowView : AView
    {
        public Transform TargetTransform;

        public float Yaw;
        public float Pitch;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, 179)]
        public float Fov;

        public GameObject centralPoint;
        public float YawOffsetMax;
        public float PitchOffsetMax;

        public override CameraConfiguration GetConfiguration() => new(CalculateYaw(), CalculatePitch(), Roll, 0, Fov, transform.position);

        private float CalculateYaw()
        {
            float centralYaw = Mathf.Atan2((centralPoint.transform.position - transform.position).normalized.x,
                                                    (centralPoint.transform.position - transform.position).normalized.z) * Mathf.Rad2Deg;
            float yaw = Mathf.Atan2((TargetTransform.position - transform.position).normalized.x,
                                                    (TargetTransform.position - transform.position).normalized.z) * Mathf.Rad2Deg;
            
            float finalYaw = yaw - centralYaw;

            while (finalYaw < 180)
                finalYaw += 360;

            while (finalYaw > 180)
                finalYaw -= 360;

            Yaw = finalYaw + centralYaw;
            return Yaw;
        }

        private float CalculatePitch()
        {
            float centralPitch = -Mathf.Asin((centralPoint.transform.position - transform.position).normalized.y) * Mathf.Rad2Deg;
            float pitch = -Mathf.Asin((TargetTransform.position - transform.position).normalized.y) * Mathf.Rad2Deg;

            float finalPitch = pitch - centralPitch;
            Mathf.Clamp(finalPitch, -PitchOffsetMax, PitchOffsetMax);

            Pitch = finalPitch + centralPitch;
            return Pitch;
        }

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
