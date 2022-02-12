namespace GameEngine
{
    using System;
    using System.Linq;
    using System.Text;
    using GameEngine.Search.KDTree;

    public class World
    {
        public readonly Entity?[,] MapTiles;
        public readonly int Height;
        public readonly int Width;

        public World(int height, int width)
        {
            Height = height;
            Width = width;
            MapTiles = new Entity[width, height];
        }

        public void Render()
        {
            //var currentCursorPosition = Console.GetCursorPosition();
            
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
            
            //Console.SetCursorPosition(currentCursorPosition.Left, currentCursorPosition.Top);
        }

        public void AddEntity(Entity entity)
        {
            MapTiles[entity.GetPosition().X, entity.GetPosition().Y] = entity;
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
    }
}