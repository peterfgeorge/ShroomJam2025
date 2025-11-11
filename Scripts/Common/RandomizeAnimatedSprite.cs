using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class RandomizeAnimatedSprite : AnimatedSprite2D {
    public void Randomize(string exclude) {
        List<string> animations = SpriteFrames.GetAnimationNames().ToList();
        animations.Remove(exclude);
        Play(animations[(int)(GD.Randi() % animations.Count)]);
    }
}
