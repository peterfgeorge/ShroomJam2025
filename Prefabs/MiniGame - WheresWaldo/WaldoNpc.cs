using Godot;
using System;

public partial class WaldoNpc : Area2D {
    [Export] public float Speed = 100f;
    [Export] public float MoveDistance = 100f;

    private Vector2 _startPos;
    private bool _movingRight = true;

    public override void _Ready()
    {
        _startPos = Position;
        InputEvent += OnInputEvent; // Detect clicks
    }

    public override void _Process(double delta)
    {
        float moveAmount = (float)(Speed * delta);
        if (_movingRight)
            Position += new Vector2(moveAmount, 0);
        else
            Position -= new Vector2(moveAmount, 0);

        if (Position.X > _startPos.X + MoveDistance)
            _movingRight = false;
        else if (Position.X < _startPos.X - MoveDistance)
            _movingRight = true;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            QueueFree(); // Delete the object when clicked
        }
    }
}
