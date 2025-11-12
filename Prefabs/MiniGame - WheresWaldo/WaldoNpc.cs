using Godot;
using System;

public partial class WaldoNpc : Area2D {
    [Export] public float Speed = 100f;
    [Export] public float MoveDistance = 100f;
    [Export] RandomizeAnimatedSprite sprite;
    private AnimatedSprite2D _anim;

    private Vector2 _startPos;
    private bool _movingRight = true;
    private bool _isPaused = false;
    private float _pauseTimer = 0f;

    public override void _Ready() {
        _startPos = Position;
        InputEvent += OnInputEvent; // Detect clicks
    }

    public override void _Process(double delta) {
        if (_isPaused) {
            _pauseTimer -= (float)delta;
            if (_pauseTimer <= 0) {
                _isPaused = false;

                // Resume walking animation
                string currentAnim = _anim.Animation;
                currentAnim = currentAnim.Replace("Walk", "").Replace("Idle", "");
                currentAnim = currentAnim.Trim();
                string newAnim = currentAnim + "Walk";
                _anim.Play(newAnim);
            }
            else
                return; // Don’t move while paused
        }

        float moveAmount = (float)(Speed * delta);
        if (_movingRight)
            Position += new Vector2(moveAmount, 0);
        else
            Position -= new Vector2(moveAmount, 0);

        if (Position.X > _startPos.X + MoveDistance)
            TryFlip(false);
        else if (Position.X < _startPos.X - MoveDistance)
            TryFlip(true);

        sprite.FlipH = !_movingRight;
    }
    
    public void TryFlip(bool newDirection)
    {
        // Only flip if direction is changing
        if (_movingRight != newDirection)
        {
            _movingRight = newDirection;

            // 40% chance to pause for 2 seconds
            if (GD.Randf() < 0.4f)
            {
                if(_anim == null)
                {
                    _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
                }
                _isPaused = true;
                _pauseTimer = (float)GD.RandRange(1.5, 3);

                GD.Print("_anim exists: " + (_anim != null));
                // plays idle animation while character is paused
                // this is called here bc it has to wait until the random character anim is chosen
                string currentAnim = _anim.Animation;
                currentAnim = currentAnim.Replace("Walk", "").Replace("Idle", "");
                currentAnim = currentAnim.Trim();
                string newAnim = currentAnim + "Idle";
                _anim.Play(newAnim);
            }
        }
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
            GD.Print("Click");
            QueueFree(); // Delete the object when clicked
            if (sprite.Animation.ToString().Contains(MiniGameWheresWaldo.currentWaldo)) {
                GameController.Instance.PassGame((int)(GameController.Instance.Game_TimeLimit - GameController.Instance.GetGameTimer()));
            }
        }
    }

    public void SetSprite(string animName) {
        sprite.Play(animName+"Walk");
    }

    public void RandomizeSprite(string exclude) {
        sprite.Randomize(exclude);
    }
}
