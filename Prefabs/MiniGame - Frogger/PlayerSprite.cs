using Godot;
using System;

public partial class PlayerSprite : Sprite2D
{
    [Export] public float BobAmplitude = 3f; // how far up/down it moves (in pixels)
    [Export] public float BobSpeed = 2f;     // how fast it bobs

    private Vector2 _startPosition;
    private float _time;

    public override void _Ready()
    {
        _startPosition = Position; // remember where the sprite started
    }

    public override void _Process(double delta)
    {
        _time += (float)delta * BobSpeed;
        float offset = Mathf.Sin(_time) * BobAmplitude;
        Position = _startPosition + new Vector2(0, offset);
    }
}