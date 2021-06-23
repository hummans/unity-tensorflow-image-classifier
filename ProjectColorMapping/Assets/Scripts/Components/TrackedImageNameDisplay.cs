using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.XR.ARFoundation;
using TMPro;
using System;

namespace Components
{
    public class TrackedImageNameDisplay : MonoBehaviour
    {
        public ARTrackedImageManager aRTrackedImageManager;
        public TextMeshProUGUI imageTrackedLabel;

        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            foreach(var trackImage in args.added)
            {
                imageTrackedLabel.text = "[ Image tracked: " + trackImage.referenceImage.name + "]";
                Debug.Log(trackImage.name);
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
