using Godot;
using System;
using System.Collections.Generic;

public partial class Waves : Node2D
{
    [Export] public float ScrollSpeed = 30f; // pixels per second
    [Export] public int Direction = 0;       // -1 = left, 0 = still, 1 = right

    private List<Sprite2D> _waveSprites = new();
    private float _waveWidth;

    public override void _Ready()
    {
        // Gather all Sprite2D children (your 3 wave images)
        foreach (Node child in GetChildren())
        {
            if (child is Sprite2D sprite)
                _waveSprites.Add(sprite);
        }

        if (_waveSprites.Count == 0)
        {
            GD.PushWarning("Waves: No Sprite2D children found.");
            return;
        }

        // Assume all wave images are the same width
        _waveWidth = _waveSprites[0].Texture.GetWidth() * _waveSprites[0].Scale.X;
    }

    public override void _Process(double delta)
    {
        if (Direction == 0 || _waveSprites.Count == 0)
            return;

        float move = ScrollSpeed * (float)delta * Direction;

        // Move all wave sprites
        foreach (Sprite2D wave in _waveSprites)
            wave.Position += new Vector2(move, 0);

        // Wrap-around logic
        foreach (Sprite2D wave in _waveSprites)
        {
            // Move to the right
            if (Direction > 0 && wave.Position.X > _waveWidth)
            {
                // Find leftmost sprite and place this one to the left of it
                Sprite2D leftmost = GetLeftmost();
                wave.Position = new Vector2(leftmost.Position.X - _waveWidth, wave.Position.Y);
            }
            // Move to the left
            else if (Direction < 0 && wave.Position.X < -_waveWidth)
            {
                // Find rightmost sprite and place this one to the right of it
                Sprite2D rightmost = GetRightmost();
                wave.Position = new Vector2(rightmost.Position.X + _waveWidth, wave.Position.Y);
            }
        }
    }

    private Sprite2D GetLeftmost()
    {
        Sprite2D leftmost = _waveSprites[0];
        foreach (var wave in _waveSprites)
            if (wave.Position.X < leftmost.Position.X)
                leftmost = wave;
        return leftmost;
    }

    private Sprite2D GetRightmost()
    {
        Sprite2D rightmost = _waveSprites[0];
        foreach (var wave in _waveSprites)
            if (wave.Position.X > rightmost.Position.X)
                rightmost = wave;
        return rightmost;
    }
}
