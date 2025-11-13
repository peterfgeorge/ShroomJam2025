using Godot;
using System;

public partial class JJCollectible : Area2D {
    [Export] int value = 1;
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

    private async void OnCharacterCollision(Node2D body) {
        BodyEntered -= OnCharacterCollision;

        JJPlayerController.currentScore += value;
        Modulate = new Color(0, 0, 0, 0);

        AudioStreamPlayer sfx = GetTree().CurrentScene.GetNode<AudioStreamPlayer>("SFX");
        sfx.Play();

        QueueFree();
    }
}
