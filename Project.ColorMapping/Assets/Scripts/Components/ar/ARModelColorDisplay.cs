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
        public ViewModel.ARObjectViewModel ARObject;
        public TrackManagerViewModel trackData;
        public Renderer[] shapesColorRender;

        private Color _defaultColor = Color.white;

        void Awake() 
        { 
            trackData.currentModelId.Value = -1;
            trackData.currentColorModel = new ReactiveProperty<ObjectModel>(new ObjectModel{
                    objectId = -1,
                    Colors = new List<Color>()
            });          
        }

        void Start()
        {
            if(trackData.currentColorModel.Value != null)
            {
                OnColorReceive(trackData.currentColorModel.Value);
            }

            trackData.currentColorModel
                .Subscribe(OnColorReceive)
                .AddTo(this);
        }

        private void OnColorReceive(ObjectModel colorObject)
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
