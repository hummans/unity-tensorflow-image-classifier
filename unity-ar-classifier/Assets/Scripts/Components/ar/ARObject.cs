using UnityEngine;
using ViewModel;

namespace Components
{
    public class ARObject : MonoBehaviour
    {
        public TrackManagerViewModel trackData;
        public ARObjectViewModel arObjectData;
        
        void Awake() 
        {
            arObjectData.objectScale = this.transform.localScale;
        }
        
        void OnEnable() => trackData.currentTrackInterfaceActive.Value = false;   
        void OnDisable() => trackData.currentTrackInterfaceActive.Value = true;
    }
}
