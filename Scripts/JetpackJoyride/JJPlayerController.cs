using Godot;
using System;

public partial class JJPlayerController : CharacterBody2D {
    [Export] private float thrust = 800f;
    [Export] private float gravity = 400f;

    public static int currentScore = 0;

    public bool enabled = true;

    public override void _Ready() {
        GameController.Instance.GameTimerTimeout += Timeout;
        Velocity += Vector2.Up * thrust * 0.2f;
    }

    public override void _PhysicsProcess(double delta) {
        if (enabled) {
            float input = Input.GetAxis("Down", "Up");

            Velocity += Vector2.Up * thrust * input * (float)delta;
            HandleGravity(delta);

            MoveAndSlide();
        }
    }


    private void HandleGravity(double delta) {
        Velocity += new Vector2(0f, gravity * (float)delta);
    }

    private void Timeout() {
        GameController.Instance.PassGame(currentScore + (int)GameController.Instance.Game_TimeLimit);
    }

    public override void _ExitTree() {
        GameController.Instance.GameTimerTimeout -= Timeout;
    }
}
