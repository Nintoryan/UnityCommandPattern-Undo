using DG.Tweening;
using UnityEngine;

public class Move : ICommand
{
    public const float MovingDuration = 0.2f;
    
    private readonly Vector3 direction;
    private readonly float distance;
    private readonly Transform objectToMove;

    public Move(Transform objectToMove, Vector3 direction, float distance = 1f)
    {
        this.direction = direction;
        this.objectToMove = objectToMove;
        this.distance = distance;
    }
    
    public void Execute()
    {
        objectToMove.DOMove(objectToMove.position + direction * distance, MovingDuration);
        objectToMove.DORotate(Quaternion.LookRotation(direction).eulerAngles,MovingDuration);
    }

    public void Undo()
    {
        objectToMove.DOMove(objectToMove.position + -1*direction * distance, MovingDuration);
        objectToMove.DORotate(Quaternion.LookRotation(direction).eulerAngles,MovingDuration);
    }

    public string GetName()
    {
        if (direction.x != 0)
        {
            return $"Go by X axis for {direction.x}";
        }
        if (direction.y != 0)
        {
            return $"Go by Y axis for {direction.x}";
        }
        if (direction.z != 0)
        {
            return $"Go by Z axis for {direction.z}";
        }
        
        return "Unkown command";
    }
}
