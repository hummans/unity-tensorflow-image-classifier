using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;

namespace Components
{
    public class ModelColorDisplay : MonoBehaviour
    {
        public TrackManager trackImageManager;
        public GameObject shapes;
        private Renderer[] _rendererObjects;

        void Start()
        {
            _rendererObjects = shapes.transform.GetComponentsInChildren<Renderer>();

            if(trackImageManager.ColorObject.Value != null)
            {
                OnColorReceive(trackImageManager.ColorObject.Value);
            } 
            else 
            {   
                List<Color> colors = new List<Color>();
                colors.Add(Color.white);
                
                trackImageManager.ColorObject = new ReactiveProperty<ColorObject>(new ColorObject{
                    Colors = colors
                });
            }

            trackImageManager.ColorObject
                .Subscribe(OnColorReceive)
                .AddTo(this);
        }

        private void OnColorReceive(ColorObject colorObject)
        {
            int count = colorObject.Colors.Count;
            int total = _rendererObjects.Length - 1;
            for (int i = 0; i < count; i++)
            {
                if(i <= total)
                    _rendererObjects[i].material.SetColor("_Color", colorObject.Colors[i]);
            }
        }
    }
}
