namespace GameEngine.Entities
{
    using System;

    public class Hill : Entity
    {
        public Hill()
        {
            RenderCharacter = '^';
        }

        public override void Update(TimeSpan gameTime)
        {
        }
    }
}