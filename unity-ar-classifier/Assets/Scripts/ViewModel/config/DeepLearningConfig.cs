using UniRx;
using UnityEngine;
using System.Collections.Generic;

namespace ViewModel
{  
    [CreateAssetMenu(fileName = "DeepLearningConfig", menuName = "Config/DeepLearningConfig", order = 0)]
    public class DeepLearningConfig : ScriptableObject 
    {
        public string modelVersion;
        public string[] recognitionResponse;
    }
}