using Godot;
using System;

public partial class Noise : TextureRect {
    RandomNumberGenerator rng = new();

    public override void _Process(double delta) {
        FastNoiseLite noise = (Texture as NoiseTexture2D).Noise as FastNoiseLite;
        noise.Offset = new Vector3(rng.RandiRange(-100, 100), rng.RandiRange(-100, 100), 0);
    }

}
