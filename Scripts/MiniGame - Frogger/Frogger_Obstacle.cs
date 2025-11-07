using Godot;
using System;

public partial class Frogger_Obstacle : Area2D
{
    [Export] int speed {get; set;} = 4;

    public override void _PhysicsProcess(double delta)
    {
        // Fixed Movement Translation
        Position += Vector2.Up * speed;
    }
}
