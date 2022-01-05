using UnityEngine;
using ViewModel;

namespace Components
{
    public class ARObject : MonoBehaviour
    {
        public ARObjectViewModel arObjectData;
        
        void Awake() 
        {
            arObjectData.objectScale = this.transform.localScale;
        }
    }
}
