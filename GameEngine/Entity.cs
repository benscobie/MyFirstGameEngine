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
        public char RenderCharacter = ' ';
        protected Point? _position { get; set; }
        protected EntityManager EntityManager { get; set; }

        public void SetEntityManager(EntityManager entityManager)
        {
            EntityManager = entityManager;
        }

        public void Spawn(Point point)
        {
            if (_position != null)
            {
                throw new InvalidOperationException("Entity has already spawned");
            }

            _position = point;

            OnCreate();
        }

        public Point? GetPosition()
        {
            return _position;
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

        public virtual void Update(TimeSpan gameTime)
        {
        }

        public virtual void OnCreate()
        {
        }

        public virtual void OnDestroy(TimeSpan gameTime)
        {
        }
    }
}