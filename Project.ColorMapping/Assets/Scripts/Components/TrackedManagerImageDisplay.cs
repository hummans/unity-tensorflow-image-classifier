using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UniRx;
using UnityEngine.XR.ARSubsystems;
using ViewModel;
using System.Linq;
using TMPro;
using System;

namespace Components
{
    public class TrackedManagerImageDisplay : MonoBehaviour
    {   
        [Header("Reference")]
        public ARTrackedImageManager ARTrackedImageManager;
        public DebugConsole debugConsole;
        
        [Header("Data")]
        public GameCmdFactory cmdGameFactory;
        public TrackManager trackData;

        private GameObject _arPrefabInstantied;
        private GameObject _cubeScreen;

        void OnEnable() => ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        void OnDisable() => ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            try
            {        
                foreach(ARTrackedImage trackedImage in eventArgs.added)
                {
                    InstanceARPrefab(trackedImage);
                }
                foreach(ARTrackedImage trackedImage in eventArgs.updated)
                {
                    if (trackedImage.trackingState == TrackingState.Tracking)
                    {
                        UpdateARPrefab(_arPrefabInstantied, trackedImage);
                    } 
                    else 
                    {
                        _arPrefabInstantied.SetActive(false);
                    }
                }
                foreach(ARTrackedImage trackedImage in eventArgs.removed)
                {
                    _arPrefabInstantied.SetActive(false);
                }
            }
            catch(Exception e)
            {   
                debugConsole.logInput.Value = "Horrible things happened! ("+e.ToString()+")";
            }
        }

        private void InstanceARPrefab(ARTrackedImage trackedImage)
        {
            if(trackData.ColorObject.Value == null) // Only execute if the server send color object data
                    return;
                    
            int idObject = trackData.ColorObject.Value.objectId;

            GameObject arObject = trackData.ARObjectPrefab
                .Where(arobject => arobject.GetComponent<ARObjectData>().ARObject.objectId == idObject)
                .FirstOrDefault();

            _arPrefabInstantied = Instantiate(arObject);

            ShowTrackInfo(trackedImage.referenceImage.name);
            UpdateARPrefab(_arPrefabInstantied, trackedImage);
        }

        private void UpdateARPrefab(GameObject prefab, ARTrackedImage trackedImage)
        {
            prefab.transform.position = trackedImage.transform.position;
            //prefab.transform.rotation = Quaternion.identity;
            //prefab.transform.localScale = trackData;
            prefab.SetActive(true);
        }

        private void ShowTrackInfo(string name)
        {
            var runtimeReferenceImageLibrary = ARTrackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;          
            debugConsole.logInput.Value = $"Go in arObjects.Values: {_arPrefabInstantied.name}";
        }
    }
}
