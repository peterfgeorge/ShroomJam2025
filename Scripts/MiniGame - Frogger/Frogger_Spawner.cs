using Godot;
using System;

public partial class Frogger_Spawner : Node2D
{
    [Export] public PackedScene ObstaclePrefab;
    [Export] float SpawnDelay = 1f;

    public bool TargetDirection_Right = true;

    Node2D[] ObstaclePool;
    int poolIndex = 0;

    public override void _Ready()
    {
        // Adjust Config for Game Round
        SpawnDelay -= GameController.Instance.GameRound * 0.05f;

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
        await ToSignal(GetTree().CreateTimer((1 + GD.Randf()) * SpawnDelay), "timeout");

        // Recurse
        SpawnLoop();
    }

}
