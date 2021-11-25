

using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ColorObject : ReactiveProperty<ColorObject>
{
    public int objectId {get; set;}
    public List<Color> Colors{get; set;}
}