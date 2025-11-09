using Godot;
using Godot.Collections;
using System;

public partial class RandomizeSprite : Sprite2D
{
    [Export] public Array<Texture2D> Sprites;

    public override void _Ready() {
        Texture = Sprites[(int) (GD.Randi() % Sprites.Count)];
    }
}
