using UnityEngine;
using UnityEngine.UI;

public class TurnBasedInput : MonoBehaviour
{
    [SerializeField] private Button _upLeft;
    [SerializeField] private Button _upRight;
    [SerializeField] private Button _downLeft;
    [SerializeField] private Button _downRight;


    [SerializeField] private Button _undo;
    [SerializeField] private Button _redo;

    [SerializeField] private TurnBasedCharacter _character;

    [SerializeField] private CommandsHandeler commandsHandeler;

    private void Awake()
    {
        _undo.enabled = false;
        
        _downLeft.onClick.AddListener(()=>SendMoveCommand(_character.transform,Vector3.left));
        _downRight.onClick.AddListener(()=>SendMoveCommand(_character.transform,Vector3.back));
        _upLeft.onClick.AddListener(()=>SendMoveCommand(_character.transform,Vector3.forward));
        _upRight.onClick.AddListener(()=>SendMoveCommand(_character.transform,Vector3.right));
        
        _undo.onClick.AddListener( () => commandsHandeler.UndoCommand());
        _redo.onClick.AddListener( () => commandsHandeler.RedoCommand());

        commandsHandeler.OnCommandStackEmpty.AddListener(() => { _undo.enabled = false;});
        commandsHandeler.OnCommandStackFull.AddListener(() => { _undo.enabled = true;});
    }

    private void SendMoveCommand(Transform objectToMove, Vector3 direction, float distance = 1f)
    {
        ICommand movement = new Move(objectToMove,direction,distance);
        commandsHandeler.AddCommand(movement);
    }
}
