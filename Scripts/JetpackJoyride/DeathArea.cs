using Godot;
using System;

public partial class DeathArea : Area2D {
    public override void _Ready() {
        BodyEntered += OnCharacterCollision;
    }

    private async void OnCharacterCollision(Node2D body) {
        BodyEntered -= OnCharacterCollision;

        GetTree().Paused = true;
        AudioStreamPlayer player = new();
        player.Stream = GD.Load<AudioStream>("res://Audio/SFX/crash.mp3");
        GetTree().CurrentScene.AddChild(player);
        player.Play();
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        GameController.Instance.FailGame(JJPlayerController.currentScore + (int)(GameController.Instance.Game_TimeLimit - GameController.Instance.GetGameTimer()));
    }
}
