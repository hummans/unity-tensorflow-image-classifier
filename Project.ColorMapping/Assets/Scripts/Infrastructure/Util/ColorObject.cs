

using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ColorObject : ReactiveProperty<ColorObject>
{
    public List<Color> Colors{get; set;}
}