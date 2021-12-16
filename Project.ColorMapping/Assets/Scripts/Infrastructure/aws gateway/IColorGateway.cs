using System;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public interface IColorGateway 
    {
        public IObservable<Unit> ColorTurn(TrackManagerViewModel trackImageManagerData, Texture2D textureScreenshoot);
    }
}