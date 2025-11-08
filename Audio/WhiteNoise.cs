using Godot;
using System;

public partial class WhiteNoise : AudioStreamPlayer {
    private AudioStreamGeneratorPlayback _playback;
    private float _sampleHz;

    public override void _Ready() {
        if (Stream is AudioStreamGenerator generator) {
            _sampleHz = generator.MixRate;
            Play();
            _playback = (AudioStreamGeneratorPlayback)GetStreamPlayback();
            FillBuffer();
        }
    }

    public override void _Process(double delta) {
        FillBuffer();
    }


    public void FillBuffer() {
        int framesAvailable = _playback.GetFramesAvailable();

        for (int i = 0; i < framesAvailable; i++) {
            Random rand = new Random();
            float sample = (float)(2.0f * (rand.NextDouble() - 0.5f));

            _playback.PushFrame(Vector2.One * sample);
        }
    }
}