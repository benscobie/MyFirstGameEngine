namespace GameEngine
{
    using System;

    public abstract class MoveableEntity : Entity
    {
        protected MovementManager MovementManager { get; set; }
        protected Entity? DestinationEntity { get; set; }
        protected Point? DestinationPoint { get; set; }
        protected int Speed { get; set; } = 5;
        protected bool AtDestination { get; set; } = true;
        private TimeSpan TimeSinceLastMovementUpdate { get; set; } = TimeSpan.Zero;

        public void SetMovementManager(MovementManager movementManager)
        {
            MovementManager = movementManager;
        }

        public void SetDestination(Entity destination)
        {
            DestinationEntity = destination;
        }
        
        public override void Update(TimeSpan gameTime)
        {
            if (DestinationEntity != null || DestinationPoint != null)
            {
                var destination = (DestinationEntity != null ? DestinationEntity.GetPosition() : DestinationPoint);

                if (destination == null) return;

                if (TimeSinceLastMovementUpdate.TotalMilliseconds > (1000 / Speed))
                {
                    var moveResult = MovementManager.Move(this, destination);

                    if (moveResult.DestinationReached || moveResult.NoPath)
                    {
                        DestinationEntity = null;
                        DestinationPoint = null;
                    }
                    
                    TimeSinceLastMovementUpdate = TimeSpan.Zero;
                }
                else
                {
                    TimeSinceLastMovementUpdate += gameTime;
                }
            }
        }
    }
}