using UniRx;
using UnityEngine;
using SimpleJSON;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New TrackImageManager", menuName = "Data/Track Image Manager")]
    public class TrackImageManager : ScriptableObject
    {
        [Header("AR Experience")]
        public GameObject arPrefab;
        public StringReactiveProperty[] rgbColors;

        [Header("Configuration")]
        public string imageNameToTrack;
        public int realWidth;
        public int realHeight;

        public ISubject<GameObject[]> OnTrackedImageChange = new Subject<GameObject[]>();
    }
}