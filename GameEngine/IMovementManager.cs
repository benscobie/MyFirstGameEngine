namespace GameEngine
{
    public interface IMovementManager
    {
        MoveResult Move(MoveableEntity entity, Point destination);
    }
}