using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New DebugConsole", menuName = "Data/Debug Console")]
    public class DebugConsole : ScriptableObject
    {
        public StringReactiveProperty consoleLabel = new StringReactiveProperty();
        public int maxToDisplay;
    }
}
