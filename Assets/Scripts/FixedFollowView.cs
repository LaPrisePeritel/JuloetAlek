using UnityEngine;

namespace TPCamera
{
    public class FixedFollowView : AView
    {
        public Transform TargetConfiguration;

        public float Yaw;
        public float Pitch;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, 179)]
        public float Fov;

        public GameObject centralPoint;
        public float FawOffsetMax;
        public float PitchOffsetMax;

        public override CameraConfiguration GetConfiguration() => new(Yaw, Pitch, Roll, 0, Fov, transform.position);

        private void Awake()
        {
            Yaw = Mathf.Atan2((TargetConfiguration.position - transform.position).normalized.x, (TargetConfiguration.position - transform.position).normalized.z) * Mathf.Rad2Deg;
            Pitch = -Mathf.Asin((TargetConfiguration.position - transform.position).normalized.y) * Mathf.Rad2Deg;
        }
    }
}
