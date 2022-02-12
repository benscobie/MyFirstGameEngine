namespace GameEngine.Entities
{
    using System;

    public class Tree : Entity
    {
        private int _lumberCount = 20;

        public Tree()
        {
            RenderCharacter = 'O';
        }

        public override void Update(TimeSpan gameTime)
        {
            
        }

        public int Chop()
        {
            _lumberCount--;

            if (_lumberCount == 0)
            {
                // TODO Destroy myself
            }

            return _lumberCount;
        }
    }
}