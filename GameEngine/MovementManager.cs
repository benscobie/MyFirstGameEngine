namespace GameEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using AStar;
    using AStar.Options;

    public class MovementManager : IMovementManager
    {
        private readonly World _world;
        private short[,] _tiles;

        public MovementManager(World world)
        {
            _world = world;
            _tiles = GenerateWorldGridTileSet();
        }

        public MoveResult Move(MoveableEntity entity, Point destination)
        {
            // TODO Probably shouldn't recalculate this each time?
            var path = GetPathToPoint(entity.GetPosition(), destination);

            if (path.Count == 0)
            {
                return new MoveResult
                {
                    NoPath = true
                };
            }

            if (path.Count == 2)
            {
                // This shouldn't really happen...
                return new MoveResult
                {
                    DestinationReached = true
                };
            }

            var nextNode = path[1];

            _world.MoveEntity(entity.GetPosition(), nextNode);
            entity.GetPosition().X = nextNode.X;
            entity.GetPosition().Y = nextNode.Y;
            _tiles[entity.GetPosition().X, entity.GetPosition().Y] = 1;
            _tiles[nextNode.X, nextNode.Y] = 0;

            // We've just moved so now we have 2 nodes left, one is where we are, the other tile we can't move into.
            if (path.Count == 3)
            {
                return new MoveResult
                {
                    DestinationReached = true,
                    Moved = true
                };
            }

            return new MoveResult
            {
                Moved = true,
            };
        }

        private IList<Point> GetPathToPoint(Point fromPoint, Point toPoint)
        {
            // TODO Listen to events where entities are added/removed and update this instead of regenerating.
            _tiles = GenerateWorldGridTileSet();

            // Set destination tile to walkable so the path finder works. We aren't actually going to move into it when we get there.
            _tiles[toPoint.X, toPoint.Y] = 1;

            var worldGrid = new WorldGrid(_tiles);
            var pathfinderOptions = new PathFinderOptions {
                UseDiagonals = true,
            };
            var pathfinder = new PathFinder(worldGrid, pathfinderOptions);
            // System.Drawing.Point wasn't working how I'd expect, using Position works if you flip row/column for X and Y for some reason...
            Position[] path = pathfinder.FindPath(new Position(fromPoint.X, fromPoint.Y), new Position(toPoint.X, toPoint.Y));
            return path.Select(p => new Point(p.Row, p.Column)).ToList();
        }

        private short[,] GenerateWorldGridTileSet()
        {
            var tiles = new short[_world.Width, _world.Height];
            
            for (var x = 0; x < _world.MapTiles.GetLength(0); x++)
            {
                for (var y = 0; y < _world.MapTiles.GetLength(1); y++)
                {
                    var entityAtPosition = _world.MapTiles[x, y];
                    if (entityAtPosition == null)
                    {
                        tiles[x, y] = 1;
                    }
                    else
                    {
                        tiles[x, y] = 0;
                    }
                    
                }
            }

            return tiles;
        }
    }
}