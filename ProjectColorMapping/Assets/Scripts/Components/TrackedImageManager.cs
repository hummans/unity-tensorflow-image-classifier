using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UniRx;
using UnityEngine.XR.ARSubsystems;
using ViewModel;
using TMPro;

namespace Components
{
    [CreateAssetMenu(fileName = "New TrackedImageManager", menuName = "Data/AR Track Image Manager")]
    public class TrackedImageManager : MonoBehaviour
    {   
        [Header("AR Reference")]
        public ARTrackedImageManager arTrackedImageManager;
        
        [Header("Data")]
        public GameCmdFactory cmdGameFactory;
        public TrackImageManager trackerManagerData;

        [Header("UI Ref")]
        public TextMeshProUGUI debugLabel;

        private GameObject _arPrefabInstantied;
        private GameObject _cubeScreen;


        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach(ARTrackedImage trackedImage in eventArgs.added)
            {
               UpdateARImage(trackedImage);
               _cubeScreen = CreateCubeForARFoundationTarget(this.gameObject, trackedImage.size.x, trackedImage.size.y, trackedImage.transform);               
               cmdGameFactory.TrackedImageChange(trackerManagerData, _arPrefabInstantied, _cubeScreen).Execute();
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

        private void UpdateARImage(ARTrackedImage trackedImage)
        {
            string imgName = trackedImage.referenceImage.name;

            _arPrefabInstantied = Instantiate(trackerManagerData.arObject);
            _arPrefabInstantied.transform.position = trackedImage.transform.position;
            _arPrefabInstantied.transform.rotation = Quaternion.identity;
            _arPrefabInstantied.transform.localScale = trackerManagerData.arScaleFactor;
            _arPrefabInstantied.SetActive(true);

            ShowTrackInfo(trackedImage.referenceImage.name);
        }

        private void ShowTrackInfo(string name)
        {
            debugLabel.text = "";
            var runtimeReferenceImageLibrary = arTrackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;  
            debugLabel.text += $"TextureFormat.RGBA32 supported: {runtimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}\n";
            debugLabel.text += $"Supported Texture Count ({runtimeReferenceImageLibrary.supportedTextureFormatCount})\n";
            
            debugLabel.text += $"trackedImage.referenceImage.name: {name}\n";
            debugLabel.text += $"Go in arObjects.Values: {_arPrefabInstantied.name}\n";
        }

        public GameObject CreateCubeForARFoundationTarget(GameObject parentObj, float targetWidth, float targetHeight, Transform trackedImage)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.SetParent(trackedImage);
            cube.GetComponent<Renderer>().material = trackerManagerData.transparentMaterial;
            cube.transform.localPosition = trackedImage.localPosition;
            cube.transform.localScale = new Vector3(targetWidth, 0.001f, targetHeight);
            debugLabel.text += $"cube.referenceScreen.create: {cube.name}\n";
            
            return cube; 
        }

        void OnEnable()
        {
            arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        void OnDisable()
        {
            arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
