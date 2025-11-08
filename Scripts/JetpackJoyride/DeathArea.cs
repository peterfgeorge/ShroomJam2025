using Godot;
using System;

public partial class DeathArea : Area2D {
    public override void _Ready() {
        BodyEntered += OnCharacterCollision;
    }

    private void OnCharacterCollision(Node2D body) {
        BodyEntered -= OnCharacterCollision;
        GameController.Instance.FailGame(JJPlayerController.currentScore + (int)(GameController.Instance.Game_TimeLimit - GameController.Instance.GetGameTimer()));
    }
}
