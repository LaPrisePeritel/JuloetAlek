using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class ViewVolumeBlender : MonoBehaviour
    {
        public static ViewVolumeBlender Instance;

        private List<AViewVolume> activeViewVolumes = new();
        private Dictionary<AView, List<AViewVolume>> volumesPerViews = new();


        public void AddVolume(AViewVolume volume)
        {
            activeViewVolumes.Add(volume);
            if (!volumesPerViews.ContainsKey(volume.View))
            {
                volumesPerViews[volume.View] = new List<AViewVolume>();
                volume.View.SetActive(true);
            }
            volumesPerViews[volume.View].Add(volume);
        }

        private void OnGUI()
        {
            foreach (AViewVolume v in activeViewVolumes)
            {
                GUILayout.Label(v.name);
            }
        }

        public void RemoveVolume(AViewVolume volume)
        {
            activeViewVolumes.Remove(volume);
            volumesPerViews[volume.View].Remove(volume);
            if (!volumesPerViews.ContainsKey(volume.View))
            {
                volumesPerViews.Remove(volume.View);
                volume.View.SetActive(false);
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);
        }
    }
}
