using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using System;
using TMPro;

public class DebugConsoleDisplay : MonoBehaviour
{
    public DebugConsole debugConsole;
    public TextMeshProUGUI consoleLabel;

    private int _current;

    void Start()
    {
        _current = debugConsole.maxToDisplay;
        debugConsole.logInput.Value = "";

        debugConsole.logInput
            .Subscribe(OnConsoleChange)
            .AddTo(this);
    }

    private void OnConsoleChange(string value)
    {
        if(_current == debugConsole.maxToDisplay)
        {
            consoleLabel.text = value;
            _current = 0;          
        } 
        else 
        {
            _current++;
            consoleLabel.text += value + "\n";
        }

        if(value != "")
        {
            Debug.Log(value);
        }
    }
}
