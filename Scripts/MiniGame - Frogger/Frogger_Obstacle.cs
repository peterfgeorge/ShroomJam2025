using Godot;
using System;

public partial class Frogger_Obstacle : Area2D
{
    [Export] float speed {get; set;} = 2;

    public bool Static = false;

    public bool TargetDirection_Right = true;

    public override void _Ready()
    {
        // Adjust Config for Game Round
        speed += GameController.Instance.GameRound * 0.1f;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (Static)
            return;

        // Fixed Movement Translation
        Position += speed * (TargetDirection_Right ? Vector2.Right : Vector2.Left);
    }
}
