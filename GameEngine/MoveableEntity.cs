namespace GameEngine
{
    using System;

    public class MoveableEntity : Entity
    {
        protected readonly MovementManager MovementManager;
        protected Entity? DestinationEntity { get; set; }
        protected Point? DestinationPoint { get; set; }
        protected int Speed { get; set; } = 5;

        protected bool AtDestination = true;
        
        private TimeSpan TimeSinceLastMovementUpdate { get; set; } = TimeSpan.Zero;
        
        public MoveableEntity(char renderCharacter, MovementManager movementManager) : base(renderCharacter)
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
                var destination = (DestinationEntity != null ? DestinationEntity.Position : DestinationPoint);

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