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

        public TextMeshProUGUI tmpLabel;

        void Start()
        {
            arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            ARTrackedImage trackedImage = null;
            GameObject arPrefab = null;
            GameObject cubeTarget = null;

            for (int i = 0; i < eventArgs.added.Count; i++)
            {
                trackedImage = eventArgs.added[i];
                string imgName = trackedImage.referenceImage.name;

                tmpLabel.text = imgName + " == " + trackerManagerData.name;

                if(imgName == trackerManagerData.name)
                {

                    arPrefab = Instantiate(trackerManagerData.arPrefab, trackedImage.transform);
                    arPrefab.SetActive(true);
                    cubeTarget = CreateCubeForARFoundationTarget(this.gameObject, trackedImage.size.x, trackedImage.size.y, trackedImage.transform);       
                    
                    cmdGameFactory.TrackedImageChange(trackerManagerData, arPrefab, cubeTarget).Execute();
                }
            }

            for (int i = 0; i < eventArgs.updated.Count; i++)
            {
                trackedImage = eventArgs.updated[i];

                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    arPrefab.SetActive(true);
                }
                else
                {
                    arPrefab.SetActive(false);
                }
            }

            for (int i = 0; i < eventArgs.removed.Count; i++)
            {
                arPrefab.SetActive(false);
            }
        }

        public GameObject CreateCubeForARFoundationTarget(GameObject parentObj, float targetWidth, float targetHeight, Transform trans)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(trans);
            cube.transform.localPosition = trans.localPosition;
            cube.transform.localScale = new Vector3(targetWidth, 0.001f, targetHeight);
            return cube; 
        }
        void OnDisable()
        {
            arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
