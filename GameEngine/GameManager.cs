namespace GameEngine
{
    using System;
    using System.Collections.Generic;

    public interface IGameManager
    {
        void ProcessInput();
        void Update(TimeSpan gameTime);
        void Render(Map map);
        IList<Entity> GetEntities();
        void AddEntity(Entity entity);
    }

    public class GameManager : IGameManager
    {
        private IList<Entity> _entities { get; } = new List<Entity>();

        public void ProcessInput()
        {
            
        }

        public IList<Entity> GetEntities()
        {
            return _entities;
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void Update(TimeSpan gameTime)
        {
            foreach (var entity in _entities)
            {
                if (entity.ShouldUpdate(gameTime))
                {
                    entity.Update(gameTime);
                }
            }
        }

        public void Render(Map map)
        {
            map.Render();
        }
    }
}