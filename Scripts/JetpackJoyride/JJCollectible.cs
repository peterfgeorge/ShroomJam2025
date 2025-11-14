using Godot;
using System;

public partial class JJCollectible : Area2D {
    [Export] int value = 2;
    [Export] int speed = 200;

    public override void _Ready() {
        AreaEntered += OnAreaEntered;
        BodyEntered += OnCharacterCollision;
        CallDeferred(nameof(CheckOverlapWithPhysics));
    }

    public override void _PhysicsProcess(double delta) {
        Position += Vector2.Left * speed * (float)delta;

        if (Position.X < -10f) {
            QueueFree();
        }
    }

    private async void OnCharacterCollision(Node2D body) {
        BodyEntered -= OnCharacterCollision;

        JJPlayerController.currentScore += value;
        Modulate = new Color(0, 0, 0, 0);

        AudioStreamPlayer sfx = GetTree().CurrentScene.GetNode<AudioStreamPlayer>("SFX");
        sfx.Play();

        QueueFree();
    }

    private void OnAreaEntered(Area2D area) {
        // Only destroy itself if it collided with an obstacle
        if (area is JJObstacle) {
            QueueFree();
        }
    }

    private void CheckOverlapWithPhysics() {
        var space = GetWorld2D().DirectSpaceState;

        CollisionShape2D shapeNode = GetNode<CollisionShape2D>("CollisionShape2D");
        if (shapeNode == null || shapeNode.Shape == null)
            return;

        PhysicsShapeQueryParameters2D query = new PhysicsShapeQueryParameters2D {
            Shape = shapeNode.Shape,
            Transform = GlobalTransform,
            CollideWithBodies = true,
            CollisionMask = 1 << 1 // <-- Make sure this matches your obstacle layer
        };

        var result = space.IntersectShape(query, 1); // max 1 result
        if (result.Count > 0) {
            QueueFree();
        }
    }

    public void CheckOverlapOnSpawn() {
        // Optional: check immediately after spawning if overlapping any collectible
        var overlappingAreas = GetOverlappingAreas();
        foreach (var area in overlappingAreas) {
            if (area is JJObstacle) {
                QueueFree();
                return;
            }
        }
    }
}
