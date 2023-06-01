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
    }
}

