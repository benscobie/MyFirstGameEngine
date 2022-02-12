// See https://aka.ms/new-console-template for more information


using System;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((_, logging) => { logging.ClearProviders(); })
    .ConfigureServices((_, services) =>
        services.AddSingleton<IGameManager, GameManager>()
            .AddLogging())
    .Build();

Run(host.Services);

await host.RunAsync();

static async void Run(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var gameManager = provider.GetRequiredService<IGameManager>();
    var previousGameTime = DateTime.UtcNow;
    var framesPerSecond = 30;
    var requiredMsPerFrame = 1000 / framesPerSecond;

    var width = Console.WindowWidth;
    var height = Console.WindowWidth;

    Console.WindowHeight = 50;
    Console.WindowWidth = 200;
    Console.CursorVisible = false;

    width = 150;
    height = 40;

    var world = new World(height, width);
    var movementManager = new MovementManager(world);
    var entityManager = new EntityManager(world, movementManager);

    for (var i = 5; i < 20; i++)
    {
        var entity = entityManager.CreateEntity<Hill>();
        entityManager.SpawnEntity(entity, new Point(i, 10));
    }

    var entityOne = entityManager.CreateEntity<Woodcutter>();
    entityManager.SpawnEntity(entityOne, new Point(5, 1));
    
    var tree = entityManager.CreateEntity<Tree>();
    entityManager.SpawnEntity(tree, new Point(25, 25));

    entityManager.FindNearestEntityOfType(entityOne.GetPosition(), typeof(Tree));
    entityOne.SetDestination(tree);
    
    gameManager.Render(world);

    while (true)
    {
        // I cheated and stole this code. Use at some point to determine the speed at which entities move.
        var actualTime = DateTime.UtcNow;
        var timePassedSinceLastLoop = actualTime - previousGameTime;
        previousGameTime += timePassedSinceLastLoop;

        gameManager.ProcessInput(entityManager);
        gameManager.Update(entityManager, timePassedSinceLastLoop);
        gameManager.Render(world);

        var processingTime = DateTime.UtcNow - actualTime;
        var sleepMs = Math.Max(requiredMsPerFrame - processingTime.Milliseconds, 0);

        //Console.WriteLine($"Processing took {processingTime.Milliseconds}ms");
        //Console.WriteLine($"Sleeping for {sleepMs}ms");
        //Console.WriteLine($"Making our frame time {processingTime.Milliseconds+sleepMs}ms");

        await Task.Delay(sleepMs);
    }
}