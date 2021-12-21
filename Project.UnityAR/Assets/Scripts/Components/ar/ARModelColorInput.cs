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
    public class ARModelColorInput : MonoBehaviour
    {
        public TrackManagerViewModel trackImageManager;
        private PlayerActionAsset playerActionAsset;

        void Awake() 
        {
            playerActionAsset = new PlayerActionAsset();    
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
                new Color32(Convert.ToByte(Random.Range(0,256)), Convert.ToByte(Random.Range(0,256)),
                 Convert.ToByte(Random.Range(0,256)), 255)
            };
                
           /* trackImageManager.currentColorModel.Value = new ARObject{
                    objectId = Random.Range(1,3),
                    Colors = colors
            };*/
        }

        private void OnDisable() 
        {
            playerActionAsset.Player.ChangeColor.started -= DoChangeColor;    
            playerActionAsset.Player.Disable();
        }
    }
}
