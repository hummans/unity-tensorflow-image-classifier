using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System;

namespace Components
{
    public class TrackedImageInformationDisplay : MonoBehaviour
    {
        public ARTrackedImageManager aRTrackedImageManager;
        public TextMeshProUGUI imageTrackedLabel;
        public TextMeshProUGUI idTrackedLabel;

        void Start() => aRTrackedImageManager.trackedImagesChanged += OnImageChange;
        void OnDisable() => aRTrackedImageManager.trackedImagesChanged -= OnImageChange;
        
        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            foreach(var trackImage in args.added)
            {
                idTrackedLabel.text = "[ Id tracked: " + trackImage.trackableId + "] ";
                imageTrackedLabel.text = "[ Image tracked: " + trackImage.referenceImage.name + "]";
            }
            foreach(var trackImage in args.updated)
            {
                idTrackedLabel.text = "[ Id tracked: " + trackImage.trackableId + "] ";
                imageTrackedLabel.text = "[ Image tracked: " + trackImage.referenceImage.name + "]";
            }
            foreach(var trackImage in args.removed)
            {
                idTrackedLabel.text = "[ Id tracked: None ] ";
                imageTrackedLabel.text = "[ Image tracked: None ]";
            }
        }
    }
}
