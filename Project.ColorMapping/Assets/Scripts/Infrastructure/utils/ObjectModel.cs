

using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ObjectModel : ReactiveProperty<ObjectModel>
{
    public int objectId {get; set;}
    public List<Color> Colors{get; set;}
}