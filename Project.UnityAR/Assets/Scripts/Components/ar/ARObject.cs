using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
