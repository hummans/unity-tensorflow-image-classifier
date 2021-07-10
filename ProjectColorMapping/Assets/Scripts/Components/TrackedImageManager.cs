using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UniRx;
using UnityEngine.XR.ARSubsystems;
using ViewModel;
using TMPro;
using System;

namespace Components
{
    public class TrackedImageManager : MonoBehaviour
    {   
        [Header("Reference")]
        public ARTrackedImageManager arTrackedImageManager;
        public DebugConsole debugConsole;
        
        [Header("Data")]
        public GameCmdFactory cmdGameFactory;
        public TrackImageManager trackerManagerData;

        private GameObject _arPrefabInstantied;
        private GameObject _cubeScreen;

        void Start() => arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        void OnDisable() => arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            try
            {
                foreach(ARTrackedImage trackedImage in eventArgs.added)
                {
                    UpdateARImage(trackedImage);
                    //_cubeScreen = CreateCubeForARFoundationTarget(this.gameObject, trackedImage.size.x, trackedImage.size.y, trackedImage.transform);               
                    //cmdGameFactory.TrackedImageChange(trackerManagerData, _arPrefabInstantied, _cubeScreen).Execute();
                }
                foreach(ARTrackedImage trackedImage in eventArgs.updated)
                {
                    if (trackedImage.trackingState == TrackingState.Tracking)
                    {
                        _arPrefabInstantied.transform.position = trackedImage.transform.position;
                        _arPrefabInstantied.transform.rotation = Quaternion.identity;
                        _arPrefabInstantied.transform.localScale = trackerManagerData.arScaleFactor;
                        _arPrefabInstantied.SetActive(true);
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

        private void UpdateARImage(ARTrackedImage trackedImage)
        {
            _arPrefabInstantied = Instantiate(trackerManagerData.arObject);
            _arPrefabInstantied.transform.position = trackedImage.transform.position;
            _arPrefabInstantied.transform.rotation = Quaternion.identity;
            _arPrefabInstantied.transform.localScale = trackerManagerData.arScaleFactor;
            _arPrefabInstantied.SetActive(true);

            ShowTrackInfo(trackedImage.referenceImage.name);
        }

        private void ShowTrackInfo(string name)
        {
            var runtimeReferenceImageLibrary = arTrackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;          
            debugConsole.logInput.Value = $"Go in arObjects.Values: {_arPrefabInstantied.name}";
        }

        public GameObject CreateCubeForARFoundationTarget(GameObject parentObj, float targetWidth, float targetHeight, Transform trackedImage)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.SetParent(trackedImage);
            cube.GetComponent<Renderer>().material = trackerManagerData.transparentMaterial;
            cube.transform.localPosition = trackedImage.localPosition;
            cube.transform.localScale = new Vector3(targetWidth, 0.001f, targetHeight);

            debugConsole.logInput.Value = $"cube.referenceScreen.create: {cube.name}"; 
            return cube; 
        }
    }
}
