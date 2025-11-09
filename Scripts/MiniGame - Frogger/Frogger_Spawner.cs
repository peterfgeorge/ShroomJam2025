using Godot;
using System;

public partial class Frogger_Spawner : Node2D
{
    [Export] public PackedScene ObstaclePrefab { get; set; }
    [Export] float SpawnDelay {get; set;} = 1f;

    public bool TargetDirection_Right = true;

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
            ((Area2D) obstacle).Monitorable = false;

            ObstaclePool[i] = obstacle;
        }

        // Begin Spawn loop
        SpawnLoop();
    }

    private async void SpawnLoop()
    {
        // Wait for Random Offset
        await ToSignal(GetTree().CreateTimer(GD.Randf()), "timeout");

        // Grab next object in object pool
        Node2D target = ObstaclePool[poolIndex++];
        if (poolIndex >= ObstaclePool.Length)
            poolIndex = 0;

        // Reset and make visible
        target.Position = Vector2.Zero;
        target.Show();
        ((Frogger_Obstacle) target).TargetDirection_Right = TargetDirection_Right;
        ((Area2D) target).Monitorable = true;

        // Wait for spawn delay
        await ToSignal(GetTree().CreateTimer(SpawnDelay), "timeout");

        // Recurse
        SpawnLoop();
    }

}
