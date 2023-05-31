using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class FixedView : AView
    {
        public float Yaw;
        public float Pitch;
        public float Roll;
        public float Fov;

        public override CameraConfiguration GetConfiguration() => new (Yaw, Pitch, Roll, 0, Fov, transform.position);
    }
}

