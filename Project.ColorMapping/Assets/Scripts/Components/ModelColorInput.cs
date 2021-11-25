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
    public class ModelColorInput : MonoBehaviour
    {
        public TrackManager trackImageManager;
        private ActionAsset playerActionAsset;

        void Awake() 
        {
            playerActionAsset = new ActionAsset();    
        }
        
        void OnEnable() 
        {
            playerActionAsset.Player.ChangeColor.started += DoChangeColor;    
            playerActionAsset.Player.Enable();
        }

        private void DoChangeColor(InputAction.CallbackContext obj)
        {
            Debug.Log("Color object changed");

            List<Color> colors = new List<Color>()
            {
                Color.yellow,
                Color.black
            };
                
            trackImageManager.ColorObject.Value = new ColorObject{
                    objectId = Random.Range(1,4),
                    Colors = colors
            };
        }

        private void OnDisable() 
        {
            playerActionAsset.Player.ChangeColor.started -= DoChangeColor;    
            playerActionAsset.Player.Disable();
        }
    }
}
