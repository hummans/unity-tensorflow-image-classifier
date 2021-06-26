using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System;

namespace Components
{
    public class TrackedImageDisplay : MonoBehaviour
    {
        public ARTrackedImageManager aRTrackedImageManager;
        public TextMeshProUGUI imageTrackedLabel;
        public TextMeshProUGUI idTrackedLabel;

        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            foreach(var trackImage in args.added)
            {
                idTrackedLabel.text = "[ Id tracked: " + trackImage.trackableId + "] ";
                imageTrackedLabel.text = "[ Image tracked: " + trackImage.referenceImage.name + "]";
            }
        }
        
        void OnEnable()
        {
            aRTrackedImageManager.trackedImagesChanged += OnImageChange;
        }
        void OnDisable()
        {
            aRTrackedImageManager.trackedImagesChanged -= OnImageChange;
        }
    }
}
