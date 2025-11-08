using Godot;
using System;

public partial class JJCollectible : Area2D {
    [Export] int value = 50;
    [Export] int speed = 200;

    public override void _Ready() {
        BodyEntered += OnCharacterCollision;
    }

    public override void _PhysicsProcess(double delta) {
        Position += Vector2.Left * speed * (float)delta;

        if (Position.X < -10f) {
            QueueFree();
        }
    }

    private void OnCharacterCollision(Node2D body) {
        JJPlayerController.currentScore += value;
        QueueFree();
    }
}
