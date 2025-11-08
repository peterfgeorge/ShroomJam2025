using Godot;
using System;

public partial class JJObstacle : DeathArea {
    [Export] int speed = 200;

    public override void _PhysicsProcess(double delta) {
        Position += Vector2.Left * speed * (float)delta;

        if (Position.X < -10f) {
            QueueFree();
        }
    }
}
