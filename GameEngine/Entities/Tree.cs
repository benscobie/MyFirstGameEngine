namespace GameEngine.Entities
{
    using System;

    public class Tree : Entity
    {
        private int _lumberCount = 20;

        public Tree(char renderCharacter = 'O') : base(renderCharacter)
        {
        }

        public override void Update(TimeSpan gameTime)
        {
            
        }
    }
}