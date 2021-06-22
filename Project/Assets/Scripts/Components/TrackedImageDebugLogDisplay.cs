using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageDebugLogDisplay : MonoBehaviour
{
        public ARTrackedImageManager aRTrackedImageManager;
        public TextMeshProUGUI debugLogLabel;

        public void ShowTrackerInfo()
        {
            if(aRTrackedImageManager != null)
            {
                var runtimeReferenceImageLibrary = aRTrackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
    
                debugLogLabel.text += $"TextureFormat.RGBA32 supported: {runtimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}\n";
                debugLogLabel.text += $"Supported Texture Count ({runtimeReferenceImageLibrary.supportedTextureFormatCount})\n";
                debugLogLabel.text += $"trackImageManager.trackables.count ({aRTrackedImageManager.trackables.count})\n";
                debugLogLabel.text += $"trackImageManager.trackedImagePrefab.name ({aRTrackedImageManager.trackedImagePrefab.name})\n";
                debugLogLabel.text += $"trackImageManager.maxNumberOfMovingImages ({aRTrackedImageManager.maxNumberOfMovingImages})\n";
                debugLogLabel.text += $"trackImageManager.supportsMutableLibrary ({aRTrackedImageManager.subsystem.subsystemDescriptor.supportsMutableLibrary})\n";
                debugLogLabel.text += $"trackImageManager.requiresPhysicalImageDimensions ({aRTrackedImageManager.subsystem.subsystemDescriptor.requiresPhysicalImageDimensions})\n";
            }
        }
        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            debugLogLabel.text = "";
            ShowTrackerInfo();
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
