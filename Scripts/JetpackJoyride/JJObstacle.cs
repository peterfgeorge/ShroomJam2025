using Godot;
using System;

public partial class JJObstacle : DeathArea
{
    [Export] int speed = 200;

    public override void _Ready()
    {
        // Connect Area2D signal to detect collisions with collectibles
        AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Left * speed * (float)delta;

        // Optional: destroy if it moves off screen
        if (Position.X < -10f)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        // Only destroy itself if it collided with a collectible
        if (area is JJCollectible)
        {
            QueueFree();
        }
    }

    public void CheckOverlapOnSpawn()
    {
        // Optional: check immediately after spawning if overlapping any collectible
        var overlappingAreas = GetOverlappingAreas();
        foreach (var area in overlappingAreas)
        {
            if (area is JJCollectible)
            {
                QueueFree();
                return;
            }
        }
    }
}
