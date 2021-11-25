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
    public class ARObjectData : MonoBehaviour
    {
        public ARObject ARObject;
        
        void Awake() 
        {
            ARObject.objectScale = this.transform.localScale;
        }
    }
}
