using Godot;
using System;

public partial class JJObstacle : Area2D {
    [Export] int speed = 200;

    public override void _Ready() {
        BodyEntered += OnCharacterCollision;
    }

    public override void _PhysicsProcess(double delta) {
        Position += Vector2.Left * speed * (float)delta;
    }

    private void OnCharacterCollision(Node2D body) {
        GD.Print("DEAD");
    }
}
