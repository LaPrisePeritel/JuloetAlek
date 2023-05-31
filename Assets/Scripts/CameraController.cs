using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TPCamera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Camera Camera;

        private List<AView> activeViews = new List<AView>();

        public CameraConfiguration CurrentConfiguration;
        public CameraConfiguration TargetConfiguration;

        [SerializeField]
        private float CameraTransitionSpeedValue;
        [SerializeField]
        private float CameraTransitionSpeedCurrent;

        private float StartDistance;



        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);

        }

        private void Start()
        {
            CurrentConfiguration = new(Camera.transform.rotation.eulerAngles.x, Camera.transform.rotation.eulerAngles.y, Camera.transform.rotation.eulerAngles.z, 0, Camera.fieldOfView, Camera.transform.position);
            SetTargetConfiguration();
        }

        private void Update()
        {
            SmoothInterpolation();
        }

        private void SmoothInterpolation()
        {
            if(CameraTransitionSpeedCurrent * Time.deltaTime > 0.01)
            {
                CurrentConfiguration.Pivot += (TargetConfiguration.GetPosition() - CurrentConfiguration.GetPosition()) * CameraTransitionSpeedValue * Time.deltaTime;
                Camera.transform.position = CurrentConfiguration.GetPosition();
                CameraTransitionSpeedCurrent = (Vector3.Distance(TargetConfiguration.GetPosition(), CurrentConfiguration.GetPosition()) / StartDistance) * CameraTransitionSpeedValue;
                //CurrentConfiguration.Yaw = CurrentConfiguration.Yaw + (TargetConfiguration.Yaw - CurrentConfiguration.Yaw) * CameraTransitionSpeedValue * Time.deltaTime;
            }
            else
            {
                CameraTransitionSpeedCurrent = 0;
                CurrentConfiguration = TargetConfiguration;
                Camera.transform.position = CurrentConfiguration.GetPosition();
                TargetConfiguration = null;
            }
            Debug.Log(CameraTransitionSpeedCurrent * Time.deltaTime);
            
        }

        public void SetTargetConfiguration()
        {
            TargetConfiguration = ComputeAverageConfiguration();
            StartDistance = Vector3.Distance(TargetConfiguration.Pivot, CurrentConfiguration.Pivot);
            CameraTransitionSpeedCurrent = CameraTransitionSpeedValue;
        }

        public void ApplyConfiguration()
        {
            CameraConfiguration cameraConfiguration = ComputeAverageConfiguration();

            Camera.transform.rotation = cameraConfiguration.GetRotation();
            Camera.transform.position = cameraConfiguration.GetPosition();
            Camera.fieldOfView = cameraConfiguration.Fov;

        }


        public void AddView(AView view) => activeViews.Add(view);

        public void RemoveView(AView view) => activeViews.Remove(view);

        public CameraConfiguration ComputeAverageConfiguration()
         => new(ComputeAverageYaw(), ComputeAveragePitch(), ComputeAverageRoll(), ComputeAverageDistance(), ComputeAverageFov(), ComputeAveragePivot());

        public float ComputeAverageYaw()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Yaw * Mathf.Deg2Rad), Mathf.Sin(configuration.Yaw * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }
        
        public float ComputeAveragePitch()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Pitch * Mathf.Deg2Rad), Mathf.Sin(configuration.Pitch * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }

        public float ComputeAverageRoll()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Roll * Mathf.Deg2Rad), Mathf.Sin(configuration.Roll * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }

        public Vector3 ComputeAveragePivot()
        {
            Vector3 sum = Vector3.zero;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Pivot * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }
        
        public float ComputeAverageDistance()
        {
            float sum = 0f;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Distance * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }
        
        public float ComputeAverageFov()
        {
            float sum = 0f;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Fov * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }
    }
}

