using Godot;
using System;

public partial class JJObstacle : DeathArea
{
    [Export] int speed = 200;

    public override void _Ready()
    {
        // Connect Area2D signal to detect collisions with collectibles
        AreaEntered += OnAreaEntered;

        BodyEntered += OnCharacterCollision;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Left * speed * (float)delta;

        if (Position.X < -10f)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is JJCollectible)
        {
            QueueFree();
        }
    }

    public void CheckOverlapOnSpawn()
    {
        var overlappingAreas = GetOverlappingAreas();
        foreach (var area in overlappingAreas)
        {
            if (area is JJCollectible)
            {
                QueueFree();
                return;
            }
        }
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
