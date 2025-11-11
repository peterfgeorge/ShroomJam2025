using Godot;
using Godot.Collections;
using System;

public partial class RandomizeAnimatedSprite : AnimatedSprite2D {
    public override void _Ready() {
        string[] animations = SpriteFrames.GetAnimationNames();
        Play(animations[(int)(GD.Randi() % animations.Length)]);
    }
}
