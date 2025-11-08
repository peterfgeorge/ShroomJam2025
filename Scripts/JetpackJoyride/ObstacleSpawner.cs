using Godot;
using System;

public partial class ObstacleSpawner : Node {
    [Export] public PackedScene obstaclePrefab;

    private const float SPAWN_DELAY_MIN = 0.5f;
    private const float SPAWN_DELAY_MAX = 2f;

    private float currentSpawnDelay = 0f;
    private float spawnTimer = 0f;

    RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Process(double delta) {
        spawnTimer += (float)delta;

        if (spawnTimer >= currentSpawnDelay) {
            float x = GetViewport().GetVisibleRect().Size.X;
            float y = rng.RandfRange(0, GetViewport().GetVisibleRect().Size.Y);
            Vector2 spawnPos = new Vector2(x, y);

            Node2D obstacle = obstaclePrefab.Instantiate<Node2D>();
            obstacle.GlobalPosition = spawnPos;
            AddChild(obstacle);

            spawnTimer = 0f;
            ResetSpawnDelay();
        }
    }

    private void ResetSpawnDelay() {
        currentSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }
}
