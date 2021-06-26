using UniRx;
using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New TrackImageManager", menuName = "Data/Track Image Manager")]
    public class TrackImageManager : ScriptableObject
    {
        [Header("AR Experience")]
        public GameObject arObject;
        public string arImageTrack;
        public Vector3 arScaleFactor;

        [Header("Configuration")]
        public Material transparentMaterial;
        public StringReactiveProperty[] rgbColors;
        public int realWidth;
        public int realHeight;

        public ISubject<GameObject[]> OnTrackedImageChange = new Subject<GameObject[]>();
    }
}