using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New TrackImageManager", menuName = "Data/Track Image Manager")]
    public class TrackManager : ScriptableObject
    {
        [Header("AR Experience")]
        public GameObject[] ARObjectPrefab;
        public Vector3 ARScaleFactor;
        public BoolReactiveProperty ARTrackedEnable;

        [Header("Configuration")]
        public Material transparentMaterial;
        public StringReactiveProperty[] rgbColors;
        public int widthTexture;
        public int heightTexture;

        [Header("Runtime")]
        public StringReactiveProperty currentTrackLabel;
        public BoolReactiveProperty currentTrackActive;
        public ReactiveProperty<Texture> currentScreenshoot;

        public ReactiveProperty<ColorObject> ColorObject;
        public ISubject<GameObject[]> OnTrackedImageChange = new Subject<GameObject[]>();

    }
}