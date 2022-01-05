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
        [Header("AR Reference")]
        public ARTrackedImageManager ARTrackedImageManager;
        public TrackManagerViewModel trackData;
        
        private GameObject _arPrefabInstantied;
        private GameObject _cubeScreen;

        void OnEnable() => ARTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        void OnDisable() => ARTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if(trackData.ARTrackedEnable.Value == false)
                return;

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
                        if(_arPrefabInstantied != null)
                            UpdateARPrefab(_arPrefabInstantied, trackedImage);
                    } 
                    else 
                    {
                        if(_arPrefabInstantied != null)
                            _arPrefabInstantied.SetActive(false);
                    }
                }
                foreach(ARTrackedImage trackedImage in eventArgs.removed)
                {
                    if(_arPrefabInstantied != null)
                        _arPrefabInstantied.SetActive(false);
                }
            }
            catch(Exception e)
            {   
                Debug.LogError("Horrible things happened! ("+e.ToString()+")");
            }
        }

        private void InstanceARPrefab(ARTrackedImage trackedImage)
        {      
            int idObject = trackData.currentRecognition.Value.prediction;
            Debug.Log($"Instance AR Prefab with name {trackData.deepLearningConfig.recognitionResponse[idObject]}");

            GameObject arObject = trackData.ARObjectPrefab
                .Where(arobject => arobject.GetComponent<ARObject>().arObjectData.objectId == idObject)
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
            Debug.Log($"Go in arObjects.Values: {_arPrefabInstantied.name}");
        }
    }
}
