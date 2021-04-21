using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CommandsHandeler : MonoBehaviour
{
    private readonly List<ICommand> commandList = new List<ICommand>();
    private readonly Queue<UnityEvent> ActualExecutionQueue = new Queue<UnityEvent>();

    [HideInInspector] public UnityEvent OnCommandStackEmpty = new UnityEvent();
    [HideInInspector] public UnityEvent OnCommandStackFull = new UnityEvent();
    
    [SerializeField] private TMP_Text commandsLog;

    private int index;
    private Coroutine ExecutionSeq;

    private void Start()
    {
        StartCoroutine(IEActualExecutionSequence());
    }

    public void AddCommand(ICommand command)
    {
        if (index < commandList.Count)
        {
            commandList.RemoveRange(index, commandList.Count - index);
        }
        commandList.Add(command);
        
        var ue = new UnityEvent();
        ue.AddListener(command.Execute);
        ActualExecutionQueue.Enqueue(ue);
        
        index++;
        OnCommandStackFull?.Invoke();
        UpdateCommandsLog();
    }

    // ReSharper disable once FunctionRecursiveOnAllPaths
    private IEnumerator IEActualExecutionSequence()
    {
        while (ActualExecutionQueue.Count > 0)
        {
            var command = ActualExecutionQueue.Dequeue();
            command.Invoke();
            yield return new WaitForSeconds(Move.MovingDuration);
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(IEActualExecutionSequence());
    }
    
    public void UndoCommand()
    {
        var ue = new UnityEvent();
        ue.AddListener(commandList[index-1].Undo);
        ActualExecutionQueue.Enqueue(ue);
        
        index--;
        if (index == 0)
        {
            OnCommandStackEmpty?.Invoke();
        }
        UpdateCommandsLog();
    }

    public void RedoCommand()
    {
        if (index < commandList.Count)
        {
            index++;
            var ue = new UnityEvent();
            ue.AddListener(commandList[index-1].Execute);
            ActualExecutionQueue.Enqueue(ue);
            OnCommandStackFull?.Invoke();
            UpdateCommandsLog();
        }
    }

    private void UpdateCommandsLog()
    {
        commandsLog.text = "Commands:\n";
        for(int i = 0; i<index; i++)
        {
            commandsLog.text += commandList[i].GetName() + "\n";
        }
    }
}
