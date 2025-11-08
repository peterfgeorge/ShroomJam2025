using Godot;
using System;

public partial class JJPlayerController : CharacterBody2D {
    [Export] private float thrust = 800f;
    [Export] private float gravity = 400f;

    public static int currentScore = 0;

    public bool enabled = false;

    public override void _Ready() {
        GameController.Instance.GameTimerTimeout += Timeout;
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
        GameController.Instance.GameTimerTimeout -= Timeout;
        GameController.Instance.PassGame(currentScore);
    }
}
