using UniRx;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New TrackImageManager", menuName = "Data/Track Image Manager")]
    public class TrackManagerViewModel : ScriptableObject
    {
        [Header("AR Experience")]
        public GameObject[] ARObjectPrefab;
        public BoolReactiveProperty ARTrackedEnable;
        public int maxNumberOfImages;
        public XRReferenceImageLibrary runtimeImageLibrary;


        [Header("Configuration")]
        public DeepLearningConfig deepLearningConfig;
        public int widthTexture;
        public int heightTexture;

        [Header("Runtime")]
        public StringReactiveProperty currentTrackLabel;
        public BoolReactiveProperty currentTrackInterfaceActive;
        public ReactiveProperty<Texture> currentTrackScreenshoot = new ReactiveProperty<Texture>();
        public ReactiveProperty<RecognitionResponse> currentRecognition = new ReactiveProperty<RecognitionResponse>();
    }
}
