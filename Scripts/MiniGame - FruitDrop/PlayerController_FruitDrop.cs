using Godot;
using System;

public partial class PlayerController_FruitDrop : Area2D
{
    [Export] float speed = 5;

    [Export] int spawn_x_offset = 149;
    [Export] int spawn_y_offset = 146;

    [Export] int GameBoundary_L = 22;
    [Export] int GameBoundary_R = 276;

    [Export] float ScorePerCatch = 0.5f;
    [Export] int AllowedDrops = 2;

    float PlayerOffset;
    int Catches = 0;

    // Game Score Calculation
    public int CalculateScore()
    {
        // Time based score - time survived
        int score_time = (int)Mathf.Floor(GameController.Instance.Game_TimeLimit - GameController.Instance.GetGameTimer());
        
        // Catches score - number of catches
        int score_catches = (int)Mathf.Floor(ScorePerCatch * Catches);

        return score_time + score_catches;
    }

    public void Collision(Area2D s)
    {        
        // Bad Object - Fail Game
        if (((Node) s).IsInGroup("FruitDrop_Bomb"))
        {
            GD.Print("FruitDrop - Bomb Collision");
            GameController.Instance.GameTimerTimeout -= Timeout;

            GameController.Instance.FailGame(CalculateScore());

            return;
        }

        // Increment Catch Count
        GD.Print("FruitDrop - Fruit Collection");
        Catches++;
        
        // Remove Object
        s.Hide();
        s.SetDeferred("monitorable", false);
    }
    public void Miss(Area2D s)
    {
        // Bomb - No Penalty
        if (((Node) s).IsInGroup("FruitDrop_Bomb"))
        {
            GD.Print("FruitDrop - Bomb Miss");
            return;
        }

        // Decrement Catch Count
        GD.Print("FruitDrop - Fruit Miss");
        GD.Print(s.Monitorable);
        Catches--;

        // Decrement lenience
        if (AllowedDrops > 0)
        {
            AllowedDrops--;
            return;
        }

        // Fail Game
        GameController.Instance.GameTimerTimeout -= Timeout;

        GameController.Instance.FailGame(CalculateScore());
    }

    public void Timeout()
    {
        // Pass Game
        GameController.Instance.GameTimerTimeout -= Timeout;

        GameController.Instance.PassGame(CalculateScore());
    }
    
    
    public override void _Ready()
    {
        // Set Player Position to Bottom Center
        PlayerOffset = ((CircleShape2D)GetChild<CollisionShape2D>(0).Shape).Radius;
        Position = new Vector2(spawn_x_offset, spawn_y_offset);

        // Bind Collision Event Handler
        AreaEntered += Collision;

        // Bind Timeout Event Handler
        GameController.Instance.GameTimerTimeout += Timeout;

        // Set Initial Game State
        Catches = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        float input_vector = Input.GetAxis("Left", "Right");
        Position += Vector2.Right * input_vector * speed;

        Position = new Vector2(
            Mathf.Clamp(Position.X, GameBoundary_L + PlayerOffset, GameBoundary_R - PlayerOffset),
            Position.Y
        );
    }

}
