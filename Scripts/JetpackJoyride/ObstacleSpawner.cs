using Godot;
using System;
using System.Collections.Generic;

public partial class ObstacleSpawner : Node {
    [Export] public PackedScene obstaclePrefab;
    [Export] public PackedScene[] collectiblePrefabs = Array.Empty<PackedScene>();

    private const float SPAWN_DELAY_MIN = 0.2f;
    private const float SPAWN_DELAY_MAX = 1.5f;

    private float obsSpawnDelay = 0f;
    private float collSpawnDelay = 0f;
    private float spawnTimer = 0f;

    public bool enabled = true;

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

                if (collectiblePrefabs != null && collectiblePrefabs.Length > 0) {
                    int idx = (int)rng.RandiRange(0, collectiblePrefabs.Length - 1);
                    PackedScene chosen = collectiblePrefabs[idx];
                    if (chosen != null) {
                        Node2D collectible = chosen.Instantiate<Node2D>();
                        collectible.GlobalPosition = spawnPos;
                        AddChild(collectible);
                    }
                }

                spawnTimer = 0f;
                ResetCollDelay();
            }
        }
    }

    private void ResetSpawnDelay() {
        obsSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }

    private void ResetCollDelay() {
        collSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN * 2f, SPAWN_DELAY_MAX * 2f);
    }
}
