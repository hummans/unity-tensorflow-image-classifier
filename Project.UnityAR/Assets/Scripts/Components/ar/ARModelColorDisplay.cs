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
    public class ARModelColorDisplay : MonoBehaviour
    {
        public ARObjectViewModel ARObject;
        public TrackManagerViewModel trackData;
        public Renderer[] shapesColorRender;

        private Color _defaultColor = Color.white;

        void Start()
        {
            /*if(trackData.currentRecognition.Value != null)
            {
                OnColorReceive(trackData.currentRecognition.Value);
            }

            trackData.currentRecognition
                .Subscribe(OnColorReceive)
                .AddTo(this);*/
        }

        private void OnColorReceive()
        {
            /*if(colorObject.idModel != ARObject.objectId)
                return;
            
            int count = shapesColorRender.Length;
            int colorReceive = colorObject.rgbColors.Count;

            for (int i = 0; i < count; i++)
            {
                if(i < colorReceive)
                    shapesColorRender[i].material.SetColor("_Color", colorObject.rgbColors[i]);
                else
                    shapesColorRender[i].material.SetColor("_Color", _defaultColor);
            }*/
        }
    }
}
