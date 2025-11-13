using Godot;
using System;
using System.Collections.Generic;

public partial class ObstacleSpawner : Node2D {
    [Export] public PackedScene obstaclePrefab;
    [Export] public PackedScene[] collectiblePrefabs = Array.Empty<PackedScene>();

    private const float SPAWN_DELAY_MIN = 0.2f;
    private const float SPAWN_DELAY_MAX = 1f;

    private float obsTimer = 0f;
    private float collTimer = 0f;

    private float obsSpawnDelay = 0f;
    private float collSpawnDelay = 0f;

    public bool enabled = true;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready() {
        ResetSpawnDelay();
        ResetCollDelay();
    }

    public override void _Process(double delta) {
        if (!enabled)
            return;

        obsTimer += (float)delta;
        collTimer += (float)delta;

        // Spawn obstacles independently
        if (obsTimer >= obsSpawnDelay) {
            SpawnObstacle();
            obsTimer = 0f;
            ResetSpawnDelay();
        }

        // Spawn collectibles independently
        if (collTimer >= collSpawnDelay) {
            SpawnCollectible();
            collTimer = 0f;
            ResetCollDelay();
        }
    }

    private void SpawnObstacle() {
        if (obstaclePrefab == null) return;

        float x = GetViewport().GetVisibleRect().Size.X;
        float y = rng.RandfRange(30, 150);
        Vector2 spawnPos = new Vector2(x, y);

        Node2D obstacle = obstaclePrefab.Instantiate<Node2D>();
        obstacle.GlobalPosition = spawnPos;
        AddChild(obstacle);
    }

    private void SpawnCollectible() {
        if (collectiblePrefabs == null || collectiblePrefabs.Length == 0)
            return;

        int attempts = 0;
        const int MAX_ATTEMPTS = 5;

        while (attempts < MAX_ATTEMPTS) {
            float x = GetViewport().GetVisibleRect().Size.X;
            float y = rng.RandfRange(30, 150);
            Vector2 spawnPos = new Vector2(x, y);

            if (!IsPositionOverlapping(spawnPos)) {
                int idx = (int)rng.RandiRange(0, collectiblePrefabs.Length - 1);
                PackedScene chosen = collectiblePrefabs[idx];
                if (chosen == null) return;

                Node2D collectible = chosen.Instantiate<Node2D>();
                collectible.GlobalPosition = spawnPos;
                AddChild(collectible);
                return;
            }

            attempts++;
        }

        GD.Print("Failed to find valid collectible spawn after several attempts.");
    }

    private bool IsPositionOverlapping(Vector2 position) {
        var spaceState = GetWorld2D().DirectSpaceState;

        // Define a small circle area around the spawn point
        CircleShape2D testShape = new CircleShape2D();
        testShape.Radius = 12; // adjust based on your collectible's collider size

        PhysicsShapeQueryParameters2D query = new PhysicsShapeQueryParameters2D {
            Shape = testShape,
            Transform = new Transform2D(0, position),
            CollideWithAreas = true,
            CollideWithBodies = true
        };

        // Perform the query
        var results = spaceState.IntersectShape(query, 1); // check for any collisions (limit 1 result)

        // If any result found, it's overlapping
        return results.Count > 0;
    }

    private void ResetSpawnDelay() {
        obsSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }

    private void ResetCollDelay() {
        collSpawnDelay = rng.RandfRange(SPAWN_DELAY_MIN, SPAWN_DELAY_MAX);
    }
}
