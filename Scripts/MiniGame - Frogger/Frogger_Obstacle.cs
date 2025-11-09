using Godot;
using System;

public partial class Frogger_Obstacle : Area2D
{
    [Export] int speed {get; set;} = 2;

    public bool TargetDirection_Right = true;

    public override void _Ready()
    {
        // Adjust Config for Game Round
        if (GameController.Instance.GameRound > 1)
            speed++;
    }
    public override void _PhysicsProcess(double delta)
    {
        // Fixed Movement Translation
        Position += speed * (TargetDirection_Right ? Vector2.Right : Vector2.Left);
    }
}
