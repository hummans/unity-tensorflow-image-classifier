using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using System;
using TMPro;

public class ConsoleDebugDisplay : MonoBehaviour
{
    public ConsoleViewModel debugConsole;
    public TextMeshProUGUI consoleLabel;

    private int _current;
    
    void Awake() 
    {
        _current = debugConsole.maxToDisplay;
        debugConsole.logInput.Value = new LogData(){
            type = "",
            body = ""
        };
    }

    void Start()
    {
        debugConsole.logInput
            .Subscribe(OnConsoleChange)
            .AddTo(this);
    }

    private void OnConsoleChange(LogData logData)
    {
        string type = logData.type;
        string log = $"[{type}] " + logData.body;
        string current = consoleLabel.text;

        if(_current == debugConsole.maxToDisplay)
        {
            consoleLabel.text = log;
            _current = 0;          
        } 
        else 
        {
            _current++;
            consoleLabel.text = log + "\n" + current + "\n";
        }
    }
}
