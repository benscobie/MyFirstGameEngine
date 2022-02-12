namespace GameEngine
{
    using System;

    public interface IEntity
    {
        bool ShouldUpdate(TimeSpan gameTime);
        void Update(TimeSpan gameTime);
    }

    public abstract class Entity : IEntity
    {
        public int UpdateDelayTicks = 0;
        public int TicksSinceLastUpdate = 0;
        public readonly char RenderCharacter;
        public Point? Position;

        public Entity(char renderCharacter)
        {
            RenderCharacter = renderCharacter;
        }
        
        public bool ShouldUpdate(TimeSpan gameTime)
        {
            // TODO This is all shit, remove
            if (TicksSinceLastUpdate < UpdateDelayTicks)
            {
                TicksSinceLastUpdate += 1;
                return false;
            }

            TicksSinceLastUpdate = 0;
            return true;
        }

        public abstract void Update(TimeSpan gameTime);
    }
}