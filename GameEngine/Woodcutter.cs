namespace GameEngine
{
    using GameEngine.Entities;

    public class Woodcutter : MoveableEntity
    {
        public Woodcutter()
        {
            RenderCharacter = 'W';
            
        }

        public override void OnCreate()
        {
            DestinationEntity = EntityManager.FindNearestEntityOfType(GetPosition(), typeof(Tree));
        }
    }
}