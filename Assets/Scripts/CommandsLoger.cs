using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandsLoger : MonoBehaviour
{
    [SerializeField] private TMP_Text commandsLog;
    [SerializeField] private CommandsHandeler _commandsHandeler;

    private void Awake()
    {
        _commandsHandeler.OnStackUpdate += UpdateCommandsLog;
    }

    private void UpdateCommandsLog(List<ICommand> commandList)
    {
        commandsLog.text = "Commands:\n";
        foreach (var command in commandList)
        {
            commandsLog.text += command.GetName() + "\n";
        }
    }
}
