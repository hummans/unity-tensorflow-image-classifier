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
    public class ModelColorDisplay : MonoBehaviour
    {
        public ARObject ARObject;
        public TrackManager trackImageManager;
        public Renderer[] shapesColorRender;

        private Color _defaultColor = Color.white;

        void Awake() 
        { 
            trackImageManager.ColorObject = new ReactiveProperty<ColorObject>(new ColorObject{
                    objectId = -1,
                    Colors = new List<Color>()
            });          
        }

        void Start()
        {
            if(trackImageManager.ColorObject.Value != null)
            {
                OnColorReceive(trackImageManager.ColorObject.Value);
            }

            trackImageManager.ColorObject
                .Subscribe(OnColorReceive)
                .AddTo(this);
        }

        private void OnColorReceive(ColorObject colorObject)
        {
            if(colorObject.objectId != ARObject.objectId)
                return;
            
            int count = shapesColorRender.Length;
            int colorReceive = colorObject.Colors.Count;

            for (int i = 0; i < count; i++)
            {
                if(i < colorReceive)
                    shapesColorRender[i].material.SetColor("_Color", colorObject.Colors[i]);
                else
                    shapesColorRender[i].material.SetColor("_Color", _defaultColor);
            }
        }
    }
}
