using System;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IPlayGateway 
    {
        string URL_DATA {get; set;}
        public IObservable<Unit> PlayTurn(TrackManager trackImageManagerData, Texture2D textureScreenshoot);
    }
}