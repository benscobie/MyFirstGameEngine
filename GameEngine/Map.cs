namespace GameEngine
{
    using System;
    using System.Linq;
    using System.Text;
    using GameEngine.Search.KDTree;

    public class Map
    {
        public readonly Entity?[,] MapTiles;
        public readonly int Height;
        public readonly int Width;

        public Map(int height, int width)
        {
            Height = height;
            Width = width;
            MapTiles = new Entity[width, height];
        }

        public void Render()
        {
            // TODO This is not-great performance. Should just re-render the specific X,Y co-ordinates when an entity moves.
            Console.SetCursorPosition(0, 0);
            
            for (int y = 0; y < Height; y++)
            {
                var lineBuffer = new StringBuilder();

                for (int x = 0; x < Width; x++)
                {
                    var entityAtPosition = MapTiles[x, y];

                    if (entityAtPosition == null)
                    {
                        lineBuffer.Append(' ');
                    }
                    else
                    {
                        lineBuffer.Append(entityAtPosition.RenderCharacter);
                    }
                }

                Console.WriteLine(lineBuffer.ToString());
            }
        }

        public void AddEntity(Entity entity)
        {
            MapTiles[entity.Position.X, entity.Position.Y] = entity;
        }

        public void MoveEntity(Point from, Point to)
        {
            if (from.X == to.X && from.Y == to.Y)
            {
                throw new InvalidOperationException("Entity cannot move to the same place...");
            }
            
            MapTiles[to.X, to.Y] = MapTiles[from.X, from.Y];
            MapTiles[from.X, from.Y] = null;
        }

        public Point GetClosestFreeTileInRelationToPoint(Point source, Point destination)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Entity? FindNearestEntityOfType(Point point, Type? entityTypeToFind)
        {
            var tree = new KdTree.KdTree<int, Entity>(2, new IntMath());
            
            for (int x = 0; x < MapTiles.GetLength(0); x++)
            {
                for (int y = 0; y < MapTiles.GetLength(1); y++)
                {
                    if (x == point.X && y == point.Y) continue;
                    var entityAtPosition = MapTiles[x, y];
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