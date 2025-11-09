using Godot;
using System;

public partial class ObstacleSpawner : Node {
    [Export] public PackedScene obstaclePrefab;
    [Export] public PackedScene collectiblePrefab;

    private const float SPAWN_DELAY_MIN = 0.5f;
    private const float SPAWN_DELAY_MAX = 2f;

    private float obsSpawnDelay = 0f;
    private float collSpawnDelay = 0f;
    private float spawnTimer = 0f;

    public bool enabled = false;

    RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready() {
        ResetSpawnDelay();
        ResetCollDelay();
    }


    public override void _Process(double delta) {
        if (enabled) {
            spawnTimer += (float)delta;

            if (spawnTimer >= obsSpawnDelay) {
                float x = GetViewport().GetVisibleRect().Size.X;
                float y = rng.RandfRange(30, 150);
                Vector2 spawnPos = new Vector2(x, y);

                Node2D obstacle = obstaclePrefab.Instantiate<Node2D>();
                obstacle.GlobalPosition = spawnPos;
                AddChild(obstacle);

                spawnTimer = 0f;
                ResetSpawnDelay();
            }

            if (spawnTimer >= collSpawnDelay) {
                float x = GetViewport().GetVisibleRect().Size.X;
                float y = rng.RandfRange(30, 150);
                Vector2 spawnPos = new Vector2(x, y);

                Node2D obstacle = collectiblePrefab.Instantiate<Node2D>();
                obstacle.GlobalPosition = spawnPos;
                AddChild(obstacle);

                spawnTimer = 0f;
                ResetCollDelay();
            }
        }
    }

    private void ResetSpawnDelay() {
        obsSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }

    private void ResetCollDelay() {
        collSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }
}
