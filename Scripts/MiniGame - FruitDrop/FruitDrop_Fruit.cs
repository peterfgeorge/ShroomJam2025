using Godot;
using System;

public partial class FruitDrop_Fruit : Area2D {
    [Export] float MaxSpeed_Vertical = 1.5f;
    [Export] float AccelerationTime = 2f; // seconds to reach max speed
    [Export] float PopUpSpeed = 50f;       // initial upward velocity (pixels/sec)
    [Export] float PopUpDuration = 0.3f;   // duration of initial upward movement

    float popUpTimer = 0f;
    bool isPopping = true;
    float currentSpeed = 0f;
    float accelerationRate = 0f;
    float Speed_Rotation_Offset;

    private AudioStreamPlayer player;

    public override void _Ready() {
        MaxSpeed_Vertical += GameController.Instance.GameRound * 0.1f;
        accelerationRate = MaxSpeed_Vertical / AccelerationTime;

        // Start pop-up
        popUpTimer = 0f;
        isPopping = true;

        player = new();
        player.Stream = GD.Load<AudioStream>("res://Audio/SFX/pop.mp3");
        player.VolumeDb = -12;
        AddChild(player);
    }

    public override void _PhysicsProcess(double delta) {
        if (isPopping) {
            // Move upward
            Position += Vector2.Up * PopUpSpeed * (float)delta;
            popUpTimer += (float)delta;

            // End pop-up after duration
            if (popUpTimer >= PopUpDuration) {
                isPopping = false;
                currentSpeed = 0f; // start downward acceleration
            }
        } else {
            // Accelerate downward
            currentSpeed += accelerationRate * (float)delta;
            if (currentSpeed > MaxSpeed_Vertical)
                currentSpeed = MaxSpeed_Vertical;

            Position += Vector2.Down * currentSpeed;
        }

        // Hide when offscreen
        if (Position.Y > 166f)
            Visible = false;
    }

    public void ResetFruit(Vector2 spawnPosition) {
        player.Play();
        GlobalPosition = spawnPosition;

        // Reactivate pop-up
        isPopping = true;
        popUpTimer = 0f;

        // Reset falling speed
        currentSpeed = 0f;

        // Make visible and re-enable collisions
        Visible = true;
        SetDeferred("monitorable", true);
        SetDeferred("monitoring", true);
    }
}
