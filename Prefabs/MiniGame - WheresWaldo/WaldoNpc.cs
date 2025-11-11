using Godot;
using System;

public partial class WaldoNpc : Area2D {
    [Export] public float Speed = 100f;
    [Export] public float MoveDistance = 100f;
    [Export] RandomizeAnimatedSprite sprite;

    private Vector2 _startPos;
    private bool _movingRight = true;

    public override void _Ready() {
        _startPos = Position;
        InputEvent += OnInputEvent; // Detect clicks
    }

    public override void _Process(double delta) {
        float moveAmount = (float)(Speed * delta);
        if (_movingRight)
            Position += new Vector2(moveAmount, 0);
        else
            Position -= new Vector2(moveAmount, 0);

        if (Position.X > _startPos.X + MoveDistance)
            _movingRight = false;
        else if (Position.X < _startPos.X - MoveDistance)
            _movingRight = true;

        sprite.FlipH = !_movingRight;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
            GD.Print("Click");
            QueueFree(); // Delete the object when clicked
            if (sprite.Animation == MiniGameWheresWaldo.currentWaldo) {
                GameController.Instance.PassGame((int)(GameController.Instance.Game_TimeLimit - GameController.Instance.GetGameTimer()));
            }
        }
    }

    public void SetSprite(string animName) {
        sprite.Play(animName);
    }

    public void RandomizeSprite(string exclude) {
        sprite.Randomize(exclude);
    }
}
