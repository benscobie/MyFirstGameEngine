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
    
    var map = new Map(height, width);
    var movementManager = new MovementManager(map);

    // TODO Perhaps the "game manager" should control the entities, and the map just hold a reference to these?

    for (var i = 5; i < 20; i++)
    {
        var entity = new Hill();
        gameManager.AddEntity(entity);
        entity.Position = new Point(i, 10);
        map.AddEntity(entity);
    }

    var entityOne = new MoveableEntity('L', movementManager);
    gameManager.AddEntity(entityOne);
    entityOne.Position = new Point(5, 1);
    map.AddEntity(entityOne);
    
    var tree = new Tree();
    tree.Position = new Point(25, 25);
    gameManager.AddEntity(tree);
    map.AddEntity(tree);

    map.FindNearestEntityOfType(entityOne.Position, typeof(Tree));
    entityOne.SetDestination(tree);
    
    gameManager.Render(map);

    while (true)
    {
        // I cheated and stole this code. Use at some point to determine the speed at which entities move.
        var actualTime = DateTime.UtcNow;
        var timePassedSinceLastLoop = actualTime - previousGameTime;
        previousGameTime += timePassedSinceLastLoop;

        gameManager.ProcessInput();
        gameManager.Update(timePassedSinceLastLoop);
        gameManager.Render(map);

        var processingTime = DateTime.UtcNow - actualTime;
        var sleepMs = Math.Max(requiredMsPerFrame - processingTime.Milliseconds, 0);

        //Console.WriteLine($"Processing took {processingTime.Milliseconds}ms");
        //Console.WriteLine($"Sleeping for {sleepMs}ms");
        //Console.WriteLine($"Making our frame time {processingTime.Milliseconds+sleepMs}ms");

        await Task.Delay(sleepMs);
    }
}