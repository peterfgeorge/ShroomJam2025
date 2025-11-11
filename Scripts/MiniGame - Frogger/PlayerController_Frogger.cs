using Godot;
using System;

public partial class PlayerController_Frogger : Area2D
{
    [Export] Frogger_SpawnerController LaneController;

    [Export] float movement_delay = 0.25f;

    float speed;

    bool buffer = true;
    bool ready = true;
    
    Vector2 inputDirection;

    // Game Score Calculation
    public int CalculateScore()
    {
        return (int)Math.Floor(GameController.Instance.GetGameTimer());
    }

    // Collision Event Handler
    public void Collision(Area2D s)
    {
        GameController.Instance.GameTimerTimeout -= Timeout;

        // Win Game
        if (((Node) s).IsInGroup("Frogger_VictoryCollision"))
        {
            GameController.Instance.PassGame(CalculateScore());
        }

        // Fail Game
        else
        {
            GameController.Instance.FailGame(0);
        }

        GameController.Instance.StopGameTimer();
    }
    // Timeout Event Handler
    public void Timeout()
    {
        GameController.Instance.GameTimerTimeout -= Timeout;
        
        GameController.Instance.FailGame(0);
    }

    public override void _Ready()
    {
        // Set Player Position to Bottom Center
        Position = new Vector2(
            (LaneController.ScreenWidth + LaneController.MarginLeft - LaneController.MarginRight) / 2,
            (LaneController.MarginTop + (5*LaneController.LaneHeight) + (LaneController.LaneHeight/2))
        );
        speed = LaneController.LaneHeight;

        // Bind Collision Event Handler
        AreaEntered += Collision;

        // Bind Timeout Event Handler
        GameController.Instance.GameTimerTimeout += Timeout;
    }

    // Async Timer to restore input buffer and movement flags
    private async void InputDelay()
    {
        // After delay, reenable input buffer
        await ToSignal(GetTree().CreateTimer(movement_delay/2), "timeout");
        buffer = true;

        // After full delay, reenable movement
        await ToSignal(GetTree().CreateTimer(movement_delay/2), "timeout");
        ready = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Buffered Check for Input
        if (buffer)
        {
            Vector2 input_vector = Input.GetVector("Left", "Right", "Up", "Down");
            if (input_vector != Vector2.Zero)
            {
                // Boundary check
                Vector2 Projection = Position + input_vector * speed;
                if (Projection.X < LaneController.ScreenWidth - LaneController.MarginRight &&
                    Projection.X > LaneController.MarginLeft &&
                    Projection.Y < LaneController.ScreenHeight - LaneController.MarginBottom &&
                    Projection.Y > LaneController.MarginTop
                )
                    inputDirection = input_vector;
            }
        }

        // On Input and after input delay
        if (inputDirection != Vector2.Zero && ready)
        {
            // Force monodirection movement, preference for vertical
            if (inputDirection.Y != 0)
                inputDirection.X = 0;

            // Manual Position Update
            Position += inputDirection * speed;
            inputDirection = Vector2.Zero;
            
            // Disable movement and buffer
            buffer = false;
            ready = false;
            InputDelay();
        }
    }
}
