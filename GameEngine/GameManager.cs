namespace GameEngine
{
    using System;
    using GameEngine.Entities;

    public interface IGameManager
    {
        void ProcessInput(EntityManager entityManager);
        void Update(EntityManager entityManager, TimeSpan gameTime);
        void Render(World world);
    }

    public class GameManager : IGameManager
    {
        public void ProcessInput(EntityManager entityManager)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.W:
                    {
                        var rnd = new Random();
                        var woodCutter = entityManager.CreateEntity<Woodcutter>();
                        entityManager.SpawnEntity(woodCutter, new Point(rnd.Next(0, 100), rnd.Next(0, 25)));
                    }
                    break;
                    
                    case ConsoleKey.T:
                    {
                        var rnd = new Random();
                        var tree = entityManager.CreateEntity<Tree>();
                        entityManager.SpawnEntity(tree, new Point(rnd.Next(0, 100), rnd.Next(0, 25)));
                    }
                        break;
                }
            }
        }

        public void Update(EntityManager entityManager, TimeSpan gameTime)
        {
            foreach (var entity in entityManager.GetAllEntities())
            {
                if (entity.ShouldUpdate(gameTime))
                {
                    entity.Update(gameTime);
                }
            }
        }

        public void Render(World world)
        {
            world.Render();
        }
    }
}