namespace GameEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameEngine.Search.KDTree;

    public class EntityManager
    {
        private List<Entity> _entities { get; } = new List<Entity>();
        public readonly World World;
        public readonly MovementManager MovementManager;

        public EntityManager(World world, MovementManager movementManager)
        {
            World = world;
            MovementManager = movementManager;
        }

        public TEntity CreateEntity<TEntity>() where TEntity : Entity, new()
        {
            var entity = new TEntity();
            
            // Don't really like this...
            entity.SetEntityManager(this);
            if (entity is MoveableEntity moveableEntity)
            {
                moveableEntity.SetMovementManager(MovementManager);
            }

            _entities.Add(entity);

            return entity;
        }

        public void SpawnEntity(Entity entity, Point point)
        {
            entity.Spawn(point);
            World.AddEntity(entity);
        }

        public void DestroyEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public List<Entity> GetAllEntities()
        {
            return _entities;
        }

        public Entity? FindNearestEntityOfType(Point point, Type? entityTypeToFind)
        {
            var tree = new KdTree.KdTree<int, Entity>(2, new IntMath());

            for (int x = 0; x < World.MapTiles.GetLength(0); x++)
            {
                for (int y = 0; y < World.MapTiles.GetLength(1); y++)
                {
                    if (x == point.X && y == point.Y) continue;
                    var entityAtPosition = World.MapTiles[x, y];
                    if (entityAtPosition == null || entityAtPosition.GetType() != entityTypeToFind) continue;

                    tree.Add(new[] { x, y }, entityAtPosition);
                }
            }

            var nodes = tree.GetNearestNeighbours(new[] { point.X, point.Y }, 1);

            if (nodes.Length != 1) return null;

            return nodes.Single().Value;
        }
    }
}