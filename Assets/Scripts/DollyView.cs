using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TPCamera
{
    public class DollyView : AView
    {

        public Transform Target;

        public float Distance;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, 179)]
        public float Fov;

        public Rail MyRail;
        public float DistanceOnRail;
        public float Speed;

        public bool IsAuto = false;

        public override CameraConfiguration GetConfiguration()
        {
            CameraConfiguration returnConfig = new(0,0,0,0,80, Vector3.zero);
            DistanceOnRail = MyRail.currentDistance;

            //Set Yaw and Pitch
            Vector3 direction = (Target.position - Camera.main.transform.position).normalized;

            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;


            if (!IsAuto)
            {
                returnConfig =  new(yaw, pitch, Roll, 0, Fov, MyRail.GetPosition(DistanceOnRail));
            }
            else
            {
                float lowestDistance = float.MaxValue;
                Vector3 targetPos = Vector3.zero;
                for (int i = 1; i < MyRail.nodes.Count; i++)
                {
                    Vector3 nearestPoint = MathUtils.GetNearestPointOnSegment(MyRail.nodes[i].transform.position, MyRail.nodes[i - 1].transform.position, Target.position);
                    float currentDistance = Vector3.Distance(nearestPoint, Target.transform.position);
                    if(lowestDistance > currentDistance)
                    {
                        lowestDistance = currentDistance;
                        targetPos = nearestPoint;
                    }
                }
                //Set Yaw and Pitch
                returnConfig = new(yaw, pitch, Roll, 0, Fov, targetPos);
            }

            return returnConfig;
        }
    }
}
