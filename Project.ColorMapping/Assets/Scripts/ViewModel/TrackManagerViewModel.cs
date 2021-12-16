using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New TrackImageManager", menuName = "Data/Track Image Manager")]
    public class TrackManagerViewModel : ScriptableObject
    {
        [Header("AR Experience")]
        public GameObject[] ARObjectPrefab;
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
        public IntReactiveProperty currentModelId;
        public ReactiveProperty<ObjectModel> currentColorModel;
        public ISubject<GameObject[]> OnTrackedImageChange = new Subject<GameObject[]>();
    }
}