using Godot;
using System;

public partial class Frogger_Spawner : Node2D
{
    [Export] public PackedScene ObstaclePrefab { get; set; }
    [Export] float SpawnDelay {get; set;} = 2f;

    Node2D[] ObstaclePool;
    int poolIndex = 0;

    public override void _Ready()
    {
        // Instantiate Pool for Obstacle Spawning
        ObstaclePool = new Node2D[5];
        for (int i = 0; i < ObstaclePool.Length; i++)
        {
            Node2D obstacle = ObstaclePrefab.Instantiate<Node2D>();
            AddChild(obstacle);
            obstacle.Hide();

            ObstaclePool[i] = obstacle;
        }

        // Begin Spawn loop
        SpawnLoop();
    }

    private async void SpawnLoop()
    {
        // Grab next object in object pool
        Node2D target = ObstaclePool[poolIndex++];
        if (poolIndex >= ObstaclePool.Length)
            poolIndex = 0;

        // Reset and make visible
        target.Position = Vector2.Zero;
        target.Show();

        // Wait for spawn delay plus random offset
        await ToSignal(GetTree().CreateTimer(SpawnDelay + GD.Randf()), "timeout");

        // Recurse
        SpawnLoop();
    }

}
