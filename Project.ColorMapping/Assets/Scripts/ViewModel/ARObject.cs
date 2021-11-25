using UnityEngine;

namespace ViewModel
{    
    [CreateAssetMenu(fileName = "New ARObject", menuName = "Scriptable/ARObject")]
    public class ARObject : ScriptableObject 
    {
        public int objectId;
        public string objectName;
        public Vector3 objectScale;
    }
}