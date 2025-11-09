using Godot;
using System;

public partial class FruitDrop_Fruit : Area2D
{
    [Export] float Speed_Rotation = 0.05f;
    [Export] float Speed_Vertical = 1f;

    float Speed_Rotation_Offset;

    public override void _Ready()
    {
        // TODO: Select random sprite, if multiple options

        Speed_Rotation_Offset = (0.5f - GD.Randf()) * Speed_Rotation;
    }
    public override void _PhysicsProcess(double delta)
    {
        // Fixed Movement Translation
        Position += Vector2.Down * Speed_Vertical;

        Rotate(Speed_Rotation_Offset);
    }
}
